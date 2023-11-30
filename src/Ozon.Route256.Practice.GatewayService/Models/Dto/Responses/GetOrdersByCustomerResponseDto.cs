namespace Ozon.Route256.Practice.GatewayService.Models.Dto.Responses;

public record GetOrdersByCustomerResponseDto(
    IEnumerable<OrderDto> Orders);