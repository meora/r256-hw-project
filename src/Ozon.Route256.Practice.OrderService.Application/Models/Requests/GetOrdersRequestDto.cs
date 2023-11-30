using Ozon.Route256.Practice.OrderService.Application.Models.Enums;

namespace Ozon.Route256.Practice.OrderService.Application.Models.Requests;

public record GetOrdersRequestDto(
    string[]? Regions,
    OrderType OrderType,
    int PageNumber,
    int PageSize,
    SortDirection? SortDirection,
    SortField? SortField);
