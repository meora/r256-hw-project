namespace Ozon.Route256.Practice.OrderService.Infrastructure.Dal.Common.Shard;

internal class Shards
{
    public const string BucketPlaceholder = "__bucket__";

    public static string GetSchemaName(uint bucketId) => $"bucket_{bucketId}";
}