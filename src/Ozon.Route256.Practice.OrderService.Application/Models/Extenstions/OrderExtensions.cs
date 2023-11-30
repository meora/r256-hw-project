using Ozon.Route256.Practice.OrderService.Application.Models.Dto;
using Ozon.Route256.Practice.OrderService.Application.Models.Enums;
using Ozon.Route256.Practice.OrderService.Domain.Entities;

namespace Ozon.Route256.Practice.OrderService.Application.Models.Extenstions;

public static class OrderExtensions
{
    public static Order ToDomain(this OrderDto dto) =>
        new(
            id: dto.Id,
            quantity: dto.Quantity,
            totalAmount: dto.TotalAmount,
            totalWeight: dto.TotalWeight,
            orderType: (Domain.Enums.OrderType)dto.OrderType,
            orderDate: dto.OrderDate,
            region: dto.Region,
            orderStatus: (Domain.Enums.OrderState)dto.OrderStatus,
            clientName: dto.ClientName,
            deliveryAddress: dto.DeliveryAddress.ToDomain(),
            phoneNumber: dto.PhoneNumber,
            customerId: dto.CustomerId);

    public static OrderDto ToDto(this Order order) =>
        new()
        {
            Id = order.Id,
            Quantity = order.Quantity,
            TotalAmount = order.TotalAmount,
            TotalWeight = order.TotalWeight,
            OrderType = (OrderType)order.OrderType,
            OrderDate = order.OrderDate,
            Region = order.Region,
            OrderStatus = (OrderState)order.OrderStatus,
            ClientName = order.ClientName,
            DeliveryAddress = order.DeliveryAddress.ToDto(),
            PhoneNumber = order.PhoneNumber,
            CustomerId = order.CustomerId
        };
}
