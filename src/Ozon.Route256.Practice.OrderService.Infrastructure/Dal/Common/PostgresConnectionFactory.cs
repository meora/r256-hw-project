using Npgsql;
using System.Data;

namespace Ozon.Route256.Practice.OrderService.Infrastructure.Dal.Common;

internal class PostgresConnectionFactory : IPostgresConnectionFactory
{
    private readonly string _connectionString;

    public PostgresConnectionFactory(
        string connectionString)
    {
        _connectionString = connectionString;
    }

    public IDbConnection GetConnection() => new NpgsqlConnection(_connectionString);
}