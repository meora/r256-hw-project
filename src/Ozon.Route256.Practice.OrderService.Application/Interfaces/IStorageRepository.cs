using Ozon.Route256.Practice.OrderService.Domain.Entities;

namespace Ozon.Route256.Practice.OrderService.Application.Interfaces;

public interface IStorageRepository
{
    Task<Storage?> FindByRegion(string region, CancellationToken token);
}
