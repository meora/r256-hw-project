namespace Ozon.Route256.Practice.OrderService.Domain.Aggregates;

public class OrderAggregate
{
    public string Region { get; init; }
    public int OrdersQuantity { get; init; }
    public float TotalAmount { get; init; }
    public float TotalWeight { get; init; }
    public int CustomersQuantity { get; init; }

    public OrderAggregate(string region,
        int ordersQuantity,
        float totalAmount,
        float totalWeight,
        int customersQuantity)
    {
        Region = region;
        OrdersQuantity = ordersQuantity;
        TotalAmount = totalAmount;
        TotalWeight = totalWeight;
        CustomersQuantity = customersQuantity;
    }
}
