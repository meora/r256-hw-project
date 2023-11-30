namespace Ozon.Route256.Practice.OrderService.Domain.Enums;

public enum OrderState
{
    Created = 1,
    SentToCustomer = 2,
    Delivered = 3,
    Lost = 4,
    Cancelled = 5
}
