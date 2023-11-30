using Ozon.Route256.Practice.OrderService.Domain.Enums;

namespace Ozon.Route256.Practice.OrderService.Domain.Entities;

public class Order
{
    public long Id { get; }
    public int Quantity { get; }
    public float TotalAmount { get; }
    public float TotalWeight { get; }
    public OrderType OrderType { get; }
    public DateTime OrderDate { get; }
    public string Region { get; }
    public OrderState OrderStatus { get; private set; }
    public string ClientName { get; }
    public Address DeliveryAddress { get; }
    public string PhoneNumber { get; }
    public long CustomerId { get; }

    public Order(long id,
        int quantity,
        float totalAmount,
        float totalWeight,
        OrderType orderType,
        DateTime orderDate,
        string region,
        OrderState orderStatus,
        string clientName,
        Address deliveryAddress,
        string phoneNumber,
        long customerId)
    {
        Id = id;
        Quantity = quantity;
        TotalAmount = totalAmount;
        TotalWeight = totalWeight;
        OrderType = orderType;
        OrderDate = orderDate;
        Region = region;
        OrderStatus = orderStatus;
        ClientName = clientName;
        DeliveryAddress = deliveryAddress;
        PhoneNumber = phoneNumber;
        CustomerId = customerId;
    }

    public Order()
    {
    }

    public bool IsCanBeCancelled()
    {
        return OrderStatus == OrderState.Delivered || OrderStatus == OrderState.Cancelled;
    }

    public void SetStatusCanceled()
    {
        SetStatus(OrderState.Cancelled);
    }

    public void SetStatus(OrderState orderStatus)
    {
        OrderStatus = orderStatus;
    }
}
