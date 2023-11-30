using Dapper;
using Ozon.Route256.Practice.OrderService.Application.Interfaces;
using Ozon.Route256.Practice.OrderService.Domain.Entities;
using Ozon.Route256.Practice.OrderService.Infrastructure.Dal.Common;

namespace Ozon.Route256.Practice.OrderService.Infrastructure.Repositories.Impl.Db;

internal class StorageDbRepository : IStorageRepository
{

    private readonly IPostgresConnectionFactory _connectionFactory;

    public StorageDbRepository(IPostgresConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory ?? throw new ArgumentNullException(nameof(connectionFactory));
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
                from storages s
                    join regions r 
                        on r.id = s.region_id
                where r.name = @region
            ";

        using var connection = _connectionFactory.GetConnection();
        var result = await connection.QueryAsync<Storage>(sql, new { region });
        return result.FirstOrDefault();
    }
}
