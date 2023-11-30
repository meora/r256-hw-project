using Ozon.Route256.Practice.Shared.Dto;

namespace Ozon.Route256.Practice.Shared.Dto.Responses;

public record GetOrdersResponseDto(
    IEnumerable<OrderDto> Orders);