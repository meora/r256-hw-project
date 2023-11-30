namespace Ozon.Route256.Practice.Shared.Dto.Requests;

public record GetOrdersByRegionRequestDto(
    string[]? Regions,
    DateTime StartDateTime);