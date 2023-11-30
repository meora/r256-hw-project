namespace Ozon.Route256.Practice.GatewayService.Models.Dto.Responses;

public record GetCustomersResponseDto(
    IEnumerable<CustomerDto> Customers);