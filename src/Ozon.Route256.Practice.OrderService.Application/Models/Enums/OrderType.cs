using NpgsqlTypes;

namespace Ozon.Route256.Practice.OrderService.Application.Models.Enums;

public enum OrderType
{
    [PgName("WebSite")]
    WebSite = 1,
    [PgName("Mobile")]
    Mobile = 2,
    [PgName("Api")]
    Api = 3
}
