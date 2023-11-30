namespace Ozon.Route256.Practice.GatewayService.Models.Dto.Responses;

public record GetOrdersResponseDto(
    IEnumerable<OrderDto> Orders);