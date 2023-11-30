using Ozon.Route256.Practice.OrderService.Application.Models.Dto;

namespace Ozon.Route256.Practice.OrderService.Application.Models.Extenstions;

public static class AddressExtensions
{
    public static Domain.Entities.Address ToDomain(this AddressDto dto) =>
        new(
            region: dto.Region,
            city: dto.City,
            street: dto.Street,
            building: dto.Building,
            apartment: dto.Apartment,
            latitude: dto.Latitude,
            longitude: dto.Longitude);

    public static AddressDto ToDto(this Domain.Entities.Address address) =>
        new()
        {
            Apartment = address.Apartment,
            Building = address.Building,
            City = address.City,
            Latitude = address.Latitude,
            Longitude = address.Longitude,
            Region = address.Region,
            Street = address.Street,
        };
}
