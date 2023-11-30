using Ozon.Route256.Practice.OrderService.Application.Models.Dto;

namespace Ozon.Route256.Practice.OrderService.Application.Models.Extenstions;

public static class CustomerExtensions
{
    public static Domain.Entities.Customer ToDomain(this CustomerDto dto) =>
        new(
        id: dto.Id,
        firstName: dto.FirstName,
        lastName: dto.LastName,
        mobileNumber: dto.MobileNumber,
        email: dto.Email,
        defaultAddress: dto.Address.ToDomain(),
        addresses: dto.Addresses.Select(a => a.ToDomain()));

    public static CustomerDto ToDto(this Domain.Entities.Customer customer) =>
        new()
        {
            Id = customer.Id,
            FirstName = customer.FirstName,
            LastName = customer.LastName,
            MobileNumber = customer.MobileNumber,
            Email = customer.Email,
            Address = customer.DefaultAddress.ToDto(),
            Addresses = customer.Addresses.Select(a => a.ToDto())
        };
}
