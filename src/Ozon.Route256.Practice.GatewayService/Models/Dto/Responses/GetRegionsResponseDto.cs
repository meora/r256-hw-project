namespace Ozon.Route256.Practice.GatewayService.Models.Dto.Responses;

public record GetRegionsResponseDto(
    IEnumerable<string> Regions);