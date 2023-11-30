using NpgsqlTypes;

namespace Ozon.Route256.Practice.OrderService.Application.Models.Enums;

public enum OrderState
{
    [PgName("Created")]
    Created = 1,
    [PgName("SentToCustomer")]
    SentToCustomer = 2,
    [PgName("Delivered")]
    Delivered = 3,
    [PgName("Lost")]
    Lost = 4,
    [PgName("Cancelled")]
    Cancelled = 5
}
