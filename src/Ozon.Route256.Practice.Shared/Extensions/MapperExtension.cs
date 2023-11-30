using Google.Protobuf.WellKnownTypes;
using Ozon.Route256.Practice.Shared.Dto;
using Ozon.Route256.Practice.Shared.Dto.Requests;

namespace Ozon.Route256.Practice.Shared.Extensions
{
    public static class MapperExtension
    {
        #region Dto to Proto

        public static Address ToProto(this AddressDto addressDto) =>
            new()
            {
                Region = addressDto.Region,
                City = addressDto.City,
                Street = addressDto.Street,
                Building = addressDto.Building,
                Apartment = addressDto.Apartment,
                Longitude = addressDto.Longitude,
                Latitude = addressDto.Latitude
            };

        public static Customer ToProto(this CustomerDto dto) =>
            new()
            {
                Id = dto.Id,
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                MobileNumber = dto.MobileNumber,
                Email = dto.Email,
                DefaultAddress = dto.DefaultAddress.ToProto(),
                Addresses = { dto.Addresses.Select(x => x.ToProto()) }
            };

        public static Order ToProto(this OrderDto dto) =>
            new()
            {
                Id = dto.Id,
                Quantity = dto.Quantity,
                TotalAmount = dto.TotalAmount,
                TotalWeight = dto.TotalWeight,
                OrderType = (OrderType)dto.OrderType,
                OrderDate = Timestamp.FromDateTime(dto.OrderDate),
                Region = dto.Region,
                OrderStatus = (OrderState)dto.OrderStatus,
                ClientName = dto.ClientName,
                DeliveryAddress = dto.DeliveryAddress.ToProto(),
                PhoneNumber = dto.PhoneNumber,
                CustomerId = dto.CustomerId,
            };

        public static GetOrdersByRegionRequest ToProto(this GetOrdersByRegionRequestDto dto) =>
            new()
            {
                StartDatetime = Timestamp.FromDateTime(dto.StartDateTime),
                Regions = { dto.Regions ?? Enumerable.Empty<string>() }
            };

        public static OrdersByRegionAggregation ToProto(this OrdersByRegionAggregationDto dto) =>
            new()
            {
                Region = dto.Region,
                OrdersQuantity = dto.OrdersQuantity,
                TotalAmount = dto.TotalAmount,
                TotalWeight = dto.TotalWeight,
                CustomersQuantity = dto.CustomersQuantity
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

        public static GetOrdersByCustomerRequest ToProto(this GetOrdersByCustomerRequestDto dto) =>
            new()
            {
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
                (Enums.OrderType)proto.OrderType,
                proto.OrderDate.ToDateTime(),
                proto.Region,
                (Enums.OrderState)proto.OrderStatus,
                proto.ClientName,
                proto.DeliveryAddress.ToDto(),
                proto.PhoneNumber,
                proto.CustomerId
            );

        public static GetOrdersRequestDto ToDto(this GetOrdersRequest proto) =>
            new(
                proto.Regions.ToArray(),
                (Enums.OrderType)proto.OrderType,
                proto.PageNumber,
                proto.PageSize,
                (Enums.SortDirection)proto.SortDirection,
                (Enums.SortField)proto.SortField
            );

        public static GetOrdersByRegionRequestDto ToDto(this GetOrdersByRegionRequest proto) =>
            new(
                proto.Regions.ToArray(),
                proto.StartDatetime.ToDateTime()
            );

        public static OrdersByRegionAggregationDto ToDto(this OrdersByRegionAggregation proto) =>
            new(
                proto.Region,
                proto.OrdersQuantity,
                proto.TotalAmount,
                proto.TotalWeight,
                proto.CustomersQuantity
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
}
