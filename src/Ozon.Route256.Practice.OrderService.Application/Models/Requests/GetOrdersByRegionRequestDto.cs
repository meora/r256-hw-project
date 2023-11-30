namespace Ozon.Route256.Practice.OrderService.Application.Models.Requests;

public record GetOrdersByRegionRequestDto(
    string[]? Regions,
    DateTime StartDateTime);