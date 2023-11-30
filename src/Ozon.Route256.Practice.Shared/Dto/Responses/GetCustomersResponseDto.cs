namespace Ozon.Route256.Practice.Shared.Dto.Responses;

public record GetCustomersResponseDto(
    IEnumerable<CustomerDto> Customers);