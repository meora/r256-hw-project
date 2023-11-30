using Ozon.Route256.Practice.OrderService.Domain.Entities;

namespace Ozon.Route256.Practice.OrderService.Application.Interfaces;

public interface IRegionRepository
{
    Task<Region[]> Get(CancellationToken token);
    Task<Region[]?> FindMany(string[] regions, CancellationToken token);
}
