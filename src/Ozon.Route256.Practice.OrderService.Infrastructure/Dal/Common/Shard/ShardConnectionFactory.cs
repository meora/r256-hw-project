using Microsoft.Extensions.Options;
using Npgsql;
using Ozon.Route256.Practice.OrderService.Infrastructure.ClientBalancing;
using System.Data;

namespace Ozon.Route256.Practice.OrderService.Infrastructure.Dal.Common.Shard;

internal class ShardConnectionFactory : IShardConnectionFactory
{
    private readonly IDbStore _dbStore;
    private readonly DbOptions _dbOptions;

    public ShardConnectionFactory(
        IDbStore dbStore,
        IOptions<DbOptions> dbOptions)
    {
        _dbStore = dbStore;
        _dbOptions = dbOptions.Value;
    }


    public IEnumerable<uint> GetAllBuckets()
    {
        for (uint bucketId = 0; bucketId < _dbStore.BucketsCount; bucketId++)
        {
            yield return bucketId;
        }
    }

    public IDbConnection GetConnectionByBucket(uint bucketId)
    {
        var endpoint = _dbStore.GetEndpointByBucket(bucketId);
        var connectionString = GetConnectionString(endpoint);
        return new ShardNpgsqlConnection(new NpgsqlConnection(connectionString), bucketId);
    }

    public IDbConnection GetRandomConnection()
    {
        Random radnom = new();
        var bucketId = radnom.Next((int)_dbStore.BucketsCount);
        return GetConnectionByBucket((uint)bucketId);
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
}