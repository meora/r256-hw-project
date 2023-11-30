using System.Data;

namespace Ozon.Route256.Practice.OrderService.Infrastructure.Dal.Common.Shard;

internal interface IShardConnectionFactory
{
    IDbConnection GetConnectionByBucket(uint bucketId);
    IDbConnection GetRandomConnection();
    IEnumerable<uint> GetAllBuckets();
}
