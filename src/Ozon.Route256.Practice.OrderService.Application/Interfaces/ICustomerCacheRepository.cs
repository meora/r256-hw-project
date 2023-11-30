using Ozon.Route256.Practice.OrderService.Domain.Entities;

namespace Ozon.Route256.Practice.OrderService.Application.Interfaces;

public interface ICustomerCacheRepository
{
    Task<Customer?> Find(long id, CancellationToken token);
    Task Insert(Customer customer, CancellationToken token);
}