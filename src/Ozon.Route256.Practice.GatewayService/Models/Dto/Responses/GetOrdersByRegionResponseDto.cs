namespace Ozon.Route256.Practice.GatewayService.Models.Dto.Responses;

public record GetOrdersByRegionResponseDto(
    IEnumerable<OrdersByRegionAggregationDto> OrdersByRegionAggregations);