namespace Ozon.Route256.Practice.GatewayService.Models.Dto.Responses;

public record OrderStatusResponseDto(
    Enums.OrderState Status);