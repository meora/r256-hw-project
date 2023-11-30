namespace Ozon.Route256.Practice.GatewayService.Models.Dto.Requests;

public record GetOrdersByCustomerRequestDto(
    DateTime StartDateTime,
    int PageNumber,
    int PageSize = 100);