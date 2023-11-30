using System.Data;

namespace Ozon.Route256.Practice.OrderService.Infrastructure.Dal.Common;

internal interface IPostgresConnectionFactory
{
    IDbConnection GetConnection();
}
