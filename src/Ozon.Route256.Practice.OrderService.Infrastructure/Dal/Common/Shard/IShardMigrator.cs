namespace Ozon.Route256.Practice.OrderService.Infrastructure.Dal.Common.Shard;

public interface IShardMigrator
{
    Task Migrate(
        CancellationToken token);
}