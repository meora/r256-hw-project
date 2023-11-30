using Google.Protobuf.WellKnownTypes;
using Ozon.Route256.Practice.GatewayService.Models.Dto;
using Ozon.Route256.Practice.GatewayService.Models.Dto.Requests;

namespace Ozon.Route256.Practice.GatewayService.Extensions
{
    public static class MapperExtension
    {
        #region Dto to Proto

        public static GetOrdersByRegionRequest ToProto(this GetOrdersByRegionRequestDto dto) =>
            new()
            {
                StartDatetime = Timestamp.FromDateTime(dto.StartDateTime),
                Regions = { dto.Regions ?? Enumerable.Empty<string>() }
            };

        public static GetOrdersRequest ToProto(this GetOrdersRequestDto dto) =>
            new()
            {
                OrderType = (OrderType)dto.OrderType,
                PageNumber = dto.PageNumber,
                PageSize = dto.PageSize,
                SortDirection = (SortDirection)dto.SortDirection,
                SortField = (SortField)dto.SortField,
                Regions = { dto.Regions ?? Enumerable.Empty<string>() }
            };

        public static GetOrdersByCustomerRequest ToProto(this GetOrdersByCustomerRequestDto dto, long id) =>
            new()
            {
                CustomerId = id,
                StartDatetime = Timestamp.FromDateTime(dto.StartDateTime),
                PageNumber = dto.PageNumber,
                PageSize = dto.PageSize
            };

        #endregion

        #region Proto to Dto

        public static AddressDto ToDto(this Address proto) =>
            new(
                proto.Region,
                proto.City,
                proto.Street,
                proto.Building,
                proto.Apartment,
                proto.Longitude,
                proto.Latitude
            );

        public static CustomerDto ToDto(this Customer proto) =>
            new(
                proto.Id,
                proto.FirstName,
                proto.LastName,
                proto.MobileNumber,
                proto.Email,
                proto.DefaultAddress.ToDto(),
                proto.Addresses.Select(x => x.ToDto())
            );

        public static OrderDto ToDto(this Order proto) =>
            new(
                proto.Id,
                proto.Quantity,
                proto.TotalAmount,
                proto.TotalWeight,
                (Models.Enums.OrderType)proto.OrderType,
                proto.OrderDate.ToDateTime(),
                proto.Region,
                (Models.Enums.OrderState)proto.OrderStatus,
                proto.ClientName,
                proto.DeliveryAddress.ToDto(),
                proto.PhoneNumber,
                proto.CustomerId
            );

        public static OrdersByRegionAggregationDto ToDto(this OrdersByRegionAggregation proto) =>
            new(
                proto.Region,
                proto.OrdersQuantity,
                proto.TotalAmount,
                proto.TotalWeight,
                proto.CustomersQuantity
            );

        #endregion
    }
}
