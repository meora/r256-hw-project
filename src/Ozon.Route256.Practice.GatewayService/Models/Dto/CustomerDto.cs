namespace Ozon.Route256.Practice.GatewayService.Models.Dto;

public record CustomerDto(
    int Id,
    string FirstName,
    string LastName,
    string MobileNumber,
    string Email,
    AddressDto DefaultAddress,
    IEnumerable<AddressDto> Addresses);