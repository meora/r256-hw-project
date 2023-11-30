using Ozon.Route256.Practice.OrderService.Application.Models.Dto;
using Ozon.Route256.Practice.OrderService.Domain.Entities;

namespace Ozon.Route256.Practice.OrderService.Application.Models.Extenstions;

public static class StorageExtensions
{
    public static Storage ToDomain(this StorageDto dto) =>
        new(
            id: dto.Id,
            regionId: dto.RegionId,
            latitude: dto.Latitude,
            longtitude: dto.Longtitude);

    public static StorageDto ToDto(this Storage storage) =>
        new()
        {
            Id = storage.Id,
            RegionId = storage.RegionId,
            Latitude = storage.Latitude,
            Longtitude = storage.Longtitude
        };
}
