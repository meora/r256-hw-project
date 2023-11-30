namespace Ozon.Route256.Practice.Shared.Dto;

public record OrderDto(
    long Id,
    int Quantity,
    float TotalAmount,
    float TotalWeight,
    Enums.OrderType OrderType,
    DateTime OrderDate,
    string Region,
    Enums.OrderState OrderStatus,
    string ClientName,
    AddressDto DeliveryAddress,
    string PhoneNumber,
    long CustomerId);