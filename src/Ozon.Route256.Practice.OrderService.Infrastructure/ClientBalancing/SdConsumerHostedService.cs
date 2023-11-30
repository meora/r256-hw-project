﻿using Grpc.Core;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Ozon.Route256.Practice.OrderService.Infrastructure.Dal.Common;

namespace Ozon.Route256.Practice.OrderService.Infrastructure.ClientBalancing;

internal sealed class SdConsumerHostedService : BackgroundService
{
    private readonly IDbStore _dbStore;
    private readonly SdService.SdServiceClient _sdServiceClient;
    private readonly ILogger<SdConsumerHostedService> _logger;
    private readonly DbOptions _dbOptions;

    public SdConsumerHostedService(
        IDbStore dbStore,
        SdService.SdServiceClient sdServiceClient,
        ILogger<SdConsumerHostedService> logger,
        IOptions<DbOptions> dbOptions)
    {
        _dbStore = dbStore;
        _sdServiceClient = sdServiceClient;
        _logger = logger;
        _dbOptions = dbOptions.Value;
    }

    protected override async Task ExecuteAsync(CancellationToken cancellationToken)
    {
        while (!cancellationToken.IsCancellationRequested)
        {
            try
            {
                var request = new DbResourcesRequest
                {
                    ClusterName = _dbOptions.ClusterName
                };

                _logger.LogInformation("Try to get db data from Service Discovery for cluster name: {ClusterName}", request.ClusterName);

                using var stream = _sdServiceClient.DbResources(request, cancellationToken: cancellationToken);
                await foreach (var response in stream.ResponseStream.ReadAllAsync(cancellationToken))
                {
                    _logger.LogInformation("Получены новые данные из SD. Timestamp {Timestamp}",
                                           response.LastUpdated.ToDateTime());

                    var endpoints = GetEndpoints(response).ToList();
                    await _dbStore.UpdateEndpointsAsync(endpoints);
                }
            }
            catch (RpcException e)
            {
                if (e.StatusCode == StatusCode.Cancelled)
                {
                    await Task.Delay(10000, cancellationToken);
                    continue;
                }

                await Task.Delay(1000, cancellationToken);
            }
        }
    }

    private IEnumerable<DbEndpoint> GetEndpoints(DbResourcesResponse response) =>
        response.Replicas.Select(replica => new DbEndpoint(
                                     $"{replica.Host}:{replica.Port}",
                                     ToDbReplica(replica.Type),
                                     replica.Buckets.Select(x => (uint)x).ToArray()
                                 ));

    private DbReplicaType ToDbReplica(Replica.Types.ReplicaType replicaType) =>
        replicaType switch
        {
            Replica.Types.ReplicaType.Master => DbReplicaType.Master,
            Replica.Types.ReplicaType.Sync => DbReplicaType.Sync,
            Replica.Types.ReplicaType.Async => DbReplicaType.Async,
            _ => throw new ArgumentOutOfRangeException(nameof(replicaType), replicaType, null)
        };
}