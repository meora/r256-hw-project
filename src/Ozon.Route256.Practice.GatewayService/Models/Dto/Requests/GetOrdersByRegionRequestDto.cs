namespace Ozon.Route256.Practice.GatewayService.Models.Dto.Requests;

public record GetOrdersByRegionRequestDto(
    string[]? Regions,
    DateTime StartDateTime);