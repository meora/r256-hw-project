namespace Ozon.Route256.Practice.OrderService.Infrastructure.Dal.Common.Shard;

internal interface IShardingRule<TShardKey>
{
    uint GetBucketId(
        TShardKey shardKey);
}