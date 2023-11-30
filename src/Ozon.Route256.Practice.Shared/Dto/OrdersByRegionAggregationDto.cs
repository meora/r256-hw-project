namespace Ozon.Route256.Practice.Shared.Dto;

public record OrdersByRegionAggregationDto(
    string Region,
    int OrdersQuantity,
    float TotalAmount,
    float TotalWeight,
    int CustomersQuantity);
