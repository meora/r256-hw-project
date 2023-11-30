using FluentMigrator.Runner;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Npgsql;
using Ozon.Route256.Practice.OrderService.Infrastructure.ClientBalancing;

namespace Ozon.Route256.Practice.OrderService.Infrastructure.Dal.Common.Shard;

internal class ShardMigrator : IShardMigrator
{
    private readonly SdService.SdServiceClient _sdServiceClient;
    private readonly DbOptions _dbOptions;
    private readonly ILogger<ShardMigrator> _logger;

    public ShardMigrator(
        SdService.SdServiceClient sdServiceClient,
        IOptions<DbOptions> dbOptions,
        ILogger<ShardMigrator> logger)
    {
        _sdServiceClient = sdServiceClient;
        _dbOptions = dbOptions.Value;
        _logger = logger;
    }

    public async Task Migrate(
        CancellationToken token)
    {
        var endpoints = await GetEndpoints(_dbOptions.ClusterName, token);

        foreach (var endpoint in endpoints)
        {
            var connectionString = GetConnectionString(endpoint);

            foreach (var bucketId in endpoint.Buckets)
            {
                var serviceProvider = CreateService(connectionString);

                using var scope = serviceProvider.CreateScope();

                var context = scope.ServiceProvider.GetRequiredService<BucketMigrationContext>();
                context.UpdateCurrentSchema(bucketId);

                _logger.LogInformation("Start to up migrations for bucket {0} in schema {1}", bucketId, context.CurrentSchema);
                var runner = scope.ServiceProvider.GetRequiredService<IMigrationRunner>();
                runner.MigrateUp();
            }
        }
    }

    private IServiceProvider CreateService(
        string connectionString)
    {
        return new ServiceCollection()
            .AddSingleton<BucketMigrationContext>()
            .AddFluentMigratorCore()
            .ConfigureRunner(
                builder => builder
                    .AddPostgres()
                    .WithGlobalConnectionString(connectionString)
                    .ScanIn(typeof(SqlMigration).Assembly).For.Migrations()
                    .ScanIn(typeof(ShardVersionTableMetaData).Assembly).For.VersionTableMetaData())
            .BuildServiceProvider();
    }

    private string GetConnectionString(DbEndpoint endpoint)
    {
        var builder = new NpgsqlConnectionStringBuilder
        {
            Host = endpoint.HostAndPort,
            Database = _dbOptions.DatabaseName,
            Username = _dbOptions.User,
            Password = _dbOptions.Password
        };
        return builder.ToString();
    }

    private async Task<DbEndpoint[]> GetEndpoints(
        string clusterName,
        CancellationToken token)
    {
        using var stream = _sdServiceClient.DbResources(
            new DbResourcesRequest { ClusterName = clusterName },
            cancellationToken: token);

        await stream.ResponseStream.MoveNext(token);
        var response = stream.ResponseStream.Current;
        var endpoints = response.Replicas
            .Where(x => x.Type is Replica.Types.ReplicaType.Master)
            .Select(
                x => new DbEndpoint(
                    $"{x.Host}:{x.Port}",
                    ToDbReplica(x.Type),
                    x.Buckets.Select(x => (uint)x).ToArray()));

        return endpoints.ToArray();
    }

    private DbReplicaType ToDbReplica(Replica.Types.ReplicaType replicaType) =>
        replicaType switch
        {
            Replica.Types.ReplicaType.Master => DbReplicaType.Master,
            Replica.Types.ReplicaType.Sync => DbReplicaType.Sync,
            Replica.Types.ReplicaType.Async => DbReplicaType.Async,
            _ => throw new ArgumentOutOfRangeException(nameof(replicaType), replicaType, null)
        };

}