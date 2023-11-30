namespace Ozon.Route256.Practice.Shared.Dto;

public record CustomerDto(
    int Id,
    string FirstName,
    string LastName,
    string MobileNumber,
    string Email,
    AddressDto DefaultAddress,
    IEnumerable<AddressDto> Addresses);