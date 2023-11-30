using Google.Protobuf.WellKnownTypes;
using Ozon.Route256.Practice.OrderService.Application.Models.Requests;

namespace Ozon.Route256.Practice.OrderService.Extensions;

public static class MapperExtension
{
    #region Dto to Proto

    public static Address ToProto(this Domain.Entities.Address address) =>
        new()
        {
            Region = address.Region,
            City = address.City,
            Street = address.Street,
            Building = address.Building,
            Apartment = address.Apartment,
            Longitude = address.Longitude,
            Latitude = address.Latitude
        };

    public static Order ToProto(this Domain.Entities.Order order) =>
        new()
        {
            Id = order.Id,
            Quantity = order.Quantity,
            TotalAmount = order.TotalAmount,
            TotalWeight = order.TotalWeight,
            OrderType = (OrderType)order.OrderType,
            OrderDate = Timestamp.FromDateTime(order.OrderDate.ToUniversalTime()),
            Region = order.Region,
            OrderStatus = (OrderState)order.OrderStatus,
            ClientName = order.ClientName,
            DeliveryAddress = order.DeliveryAddress.ToProto(),
            PhoneNumber = order.PhoneNumber,
            CustomerId = order.CustomerId,
        };

    public static OrdersByRegionAggregation ToProto(this Domain.Aggregates.OrderAggregate orderAggregate) =>
        new()
        {
            Region = orderAggregate.Region,
            OrdersQuantity = orderAggregate.OrdersQuantity,
            TotalAmount = orderAggregate.TotalAmount,
            TotalWeight = orderAggregate.TotalWeight,
            CustomersQuantity = orderAggregate.CustomersQuantity
        };

    #endregion

    #region Proto to Domain

    public static Domain.Entities.Address ToDto(this Address proto) =>
        new(
            proto.Region,
            proto.City,
            proto.Street,
            proto.Building,
            proto.Apartment,
            proto.Longitude,
            proto.Latitude
        );


    public static GetOrdersRequestDto ToDto(this GetOrdersRequest proto) =>
        new(
            proto.Regions.ToArray(),
            (Application.Models.Enums.OrderType)proto.OrderType,
            proto.PageNumber,
            proto.PageSize,
            (Application.Models.Enums.SortDirection)proto.SortDirection,
            (Application.Models.Enums.SortField)proto.SortField
        );

    public static GetOrdersByRegionRequestDto ToDto(this GetOrdersByRegionRequest proto) =>
        new(
            proto.Regions.ToArray(),
            proto.StartDatetime.ToDateTime()
        );

    public static GetOrdersByCustomerRequestDto ToDto(this GetOrdersByCustomerRequest proto) =>
        new(
            proto.CustomerId,
            proto.StartDatetime.ToDateTime(),
            proto.PageNumber,
            proto.PageSize
        );

    #endregion
}
