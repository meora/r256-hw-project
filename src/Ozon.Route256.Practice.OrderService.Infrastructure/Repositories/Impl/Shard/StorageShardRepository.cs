using Dapper;
using Ozon.Route256.Practice.OrderService.Application.Interfaces;
using Ozon.Route256.Practice.OrderService.Domain.Entities;
using Ozon.Route256.Practice.OrderService.Infrastructure.Dal.Common.Shard;

namespace Ozon.Route256.Practice.OrderService.Infrastructure.Repositories.Impl.Shard;

internal class StorageShardRepository : BaseShardRepository, IStorageRepository
{
    private const string Table = $"{Shards.BucketPlaceholder}.storages";
    private const string RegionsTable = $"{Shards.BucketPlaceholder}.regions";

    public StorageShardRepository(
        IShardConnectionFactory connectionFactory,
        IShardingRule<long> longShardingRule,
        IShardingRule<string> stringShardingRule) : base(connectionFactory, longShardingRule, stringShardingRule)
    {
    }

    public async Task<Storage?> FindByRegion(string region, CancellationToken token)
    {
        token.ThrowIfCancellationRequested();

        const string sql = @$"
                select 
                    s.id, 
                    s.region_id, 
                    s.latitude, 
                    s.longtitude
                from {Table} s
                    join {RegionsTable} r 
                        on r.id = s.region_id
                where r.name = @region
            ";

        using var connection = await GetRandomConnection(token);
        var result = await connection.QueryAsync<Storage>(sql, new { region });
        return result.FirstOrDefault();
    }
}
