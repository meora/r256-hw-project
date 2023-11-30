namespace Ozon.Route256.Practice.OrderService.Infrastructure.ClientBalancing;

internal interface IDbStore
{
    Task UpdateEndpointsAsync(IReadOnlyCollection<DbEndpoint> dbEndpoints);

    DbEndpoint GetEndpointByBucket(
        uint bucketId);

    uint BucketsCount { get; }
}
