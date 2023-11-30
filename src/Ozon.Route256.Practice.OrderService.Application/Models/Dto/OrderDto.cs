using Ozon.Route256.Practice.OrderService.Application.Models.Enums;

namespace Ozon.Route256.Practice.OrderService.Application.Models.Dto;

public class OrderDto
{
    public long Id { get; init; }
    public int Quantity { get; init; }
    public float TotalAmount { get; init; }
    public float TotalWeight { get; init; }
    public OrderType OrderType { get; init; }
    public DateTime OrderDate { get; init; }
    public string Region { get; init; }
    public OrderState OrderStatus { get; set; }
    public string ClientName { get; init; }
    public AddressDto DeliveryAddress { get; init; }
    public string PhoneNumber { get; init; }
    public long CustomerId { get; init; }

    public OrderDto(long id,
        int quantity,
        float totalAmount,
        float totalWeight,
        OrderType orderType,
        DateTime orderDate,
        string region,
        OrderState orderStatus,
        string clientName,
        AddressDto deliveryAddress,
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

    public OrderDto()
    {
    }
}