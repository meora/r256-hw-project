namespace Ozon.Route256.Practice.Shared.Dto.Requests;

public record GetOrdersByCustomerRequestDto(
    long CustomerId,
    DateTime StartDateTime,
    int PageNumber,
    int PageSize = 100);