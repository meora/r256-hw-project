namespace Ozon.Route256.Practice.OrderService.Application.Models.Requests;

public record GetOrdersByCustomerRequestDto(
    long CustomerId,
    DateTime StartDateTime,
    int PageNumber,
    int PageSize = 100);