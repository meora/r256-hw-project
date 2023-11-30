using Dapper;
using Ozon.Route256.Practice.OrderService.Application.Interfaces;
using Ozon.Route256.Practice.OrderService.Domain.Entities;
using Ozon.Route256.Practice.OrderService.Infrastructure.Dal.Common.Shard;

namespace Ozon.Route256.Practice.OrderService.Infrastructure.Repositories.Impl.Shard;

internal class RegionShardRepository : BaseShardRepository, IRegionRepository
{
    private const string Table = $"{Shards.BucketPlaceholder}.regions";
    private const string Fields = "id, name";

    public RegionShardRepository(
        IShardConnectionFactory connectionFactory,
        IShardingRule<long> longShardingRule,
        IShardingRule<string> stringShardingRule) : base(connectionFactory, longShardingRule, stringShardingRule)
    {
    }

    public async Task<Region[]> Get(CancellationToken token)
    {
        token.ThrowIfCancellationRequested();

        const string sql = @$"
                select {Fields}
                from {Table}
            ";

        using var connection = await GetRandomConnection(token);
        var result = await connection.QueryAsync<Region>(sql);

        return result.ToArray();
    }

    public async Task<Region[]?> FindMany(string[] regions, CancellationToken token)
    {
        token.ThrowIfCancellationRequested();

        const string sql = @$"
                select {Fields}
                from {Table}
                where name = any(@regions::text[])
            ";

        using var connection = await GetRandomConnection(token);
        var result = await connection.QueryAsync<Region>(sql, new { regions });

        return result.ToArray();
    }
}
