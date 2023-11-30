using Ozon.Route256.Practice.OrderService.Domain.Entities;

namespace Ozon.Route256.Practice.OrderService.Application.Interfaces;

public interface ICustomerCacheService
{
    Task<Customer?> Find(long id, CancellationToken token);
}
