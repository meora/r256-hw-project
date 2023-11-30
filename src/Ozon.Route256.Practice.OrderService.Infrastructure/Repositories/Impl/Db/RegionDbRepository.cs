using Dapper;
using Ozon.Route256.Practice.OrderService.Application.Interfaces;
using Ozon.Route256.Practice.OrderService.Domain.Entities;
using Ozon.Route256.Practice.OrderService.Infrastructure.Dal.Common;

namespace Ozon.Route256.Practice.OrderService.Infrastructure.Repositories.Impl.Db;

internal class RegionDbRepository : IRegionRepository
{
    private const string Table = "regions";
    private const string Fields = "id, name";

    private readonly IPostgresConnectionFactory _connectionFactory;

    public RegionDbRepository(IPostgresConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory ?? throw new ArgumentNullException(nameof(connectionFactory));
    }

    public async Task<Region[]> Get(CancellationToken token)
    {
        token.ThrowIfCancellationRequested();

        const string sql = @$"
                select {Fields}
                from {Table}
            ";

        using var connection = _connectionFactory.GetConnection();
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

        using var connection = _connectionFactory.GetConnection();
        var result = await connection.QueryAsync<Region>(sql, new { regions });

        return result.ToArray();
    }
}
