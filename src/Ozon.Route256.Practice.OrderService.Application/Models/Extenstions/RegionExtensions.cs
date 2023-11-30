using Ozon.Route256.Practice.OrderService.Application.Models.Dto;
using Ozon.Route256.Practice.OrderService.Domain.Entities;

namespace Ozon.Route256.Practice.OrderService.Application.Models.Extenstions;

public static class RegionExtensions
{
    public static Region ToDomain(this RegionDto dto) =>
        new(
            id: dto.Id,
            name: dto.Name);

    public static RegionDto ToDto(this Region region) =>
        new()
        {
            Id = region.Id,
            Name = region.Name
        };
}
