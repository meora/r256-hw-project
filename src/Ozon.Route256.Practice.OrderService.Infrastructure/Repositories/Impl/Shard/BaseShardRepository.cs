using Ozon.Route256.Practice.OrderService.Infrastructure.Dal.Common.Shard;
using System.Data;

namespace Ozon.Route256.Practice.OrderService.Infrastructure.Repositories.Impl.Shard;

internal class BaseShardRepository
{

    protected readonly IShardConnectionFactory _connectionFactory;
    protected readonly IShardingRule<long> _longShardingRule;
    private readonly IShardingRule<string> _stringShardingRule;

    public BaseShardRepository(
        IShardConnectionFactory connectionFactory,
        IShardingRule<long> longShardingRule,
        IShardingRule<string> stringShardingRule)
    {
        _connectionFactory = connectionFactory;
        _longShardingRule = longShardingRule;
        _stringShardingRule = stringShardingRule;
    }

    protected IDbConnection GetConnectionByShardKey(
        long shardKey)
    {
        var bucketId = _longShardingRule.GetBucketId(shardKey);
        var connection = _connectionFactory.GetConnectionByBucket(bucketId);
        return connection;
    }

    protected IDbConnection GetConnectionBySearchKey(
        string searchKey)
    {
        var bucketId = _stringShardingRule.GetBucketId(searchKey);
        return _connectionFactory.GetConnectionByBucket(bucketId);
    }

    protected async Task<IDbConnection> GetConnectionByBucket(
        uint bucketId,
        CancellationToken token)
    {
        var connection = _connectionFactory.GetConnectionByBucket(bucketId);
        return connection;
    }

    protected async Task<IDbConnection> GetRandomConnection(
        CancellationToken token)
    {
        var connection = _connectionFactory.GetRandomConnection();
        return connection;
    }

}
