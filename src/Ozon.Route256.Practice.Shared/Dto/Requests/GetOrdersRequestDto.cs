namespace Ozon.Route256.Practice.Shared.Dto.Requests;

public record GetOrdersRequestDto(
    string[]? Regions,
    Enums.OrderType OrderType,
    int PageNumber,
    int PageSize,
    Enums.SortDirection? SortDirection,
    Enums.SortField? SortField);
