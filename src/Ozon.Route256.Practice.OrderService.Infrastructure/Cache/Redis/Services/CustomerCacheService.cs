using Ozon.Route256.Practice.OrderService.Application.Interfaces;
using Ozon.Route256.Practice.OrderService.Infrastructure.Extensions;

namespace Ozon.Route256.Practice.OrderService.Infrastructure.Cache.Redis.Services;

internal class CustomerCacheService : ICustomerCacheService
{
    private readonly Customers.CustomersClient _customersClient;
    private readonly ICustomerCacheRepository _customerCache;

    public CustomerCacheService(Customers.CustomersClient customersClient, ICustomerCacheRepository customerCache)
    {
        _customersClient = customersClient;
        _customerCache = customerCache;
    }
    public async Task<Domain.Entities.Customer?> Find(long id, CancellationToken token)
    {
        var customer = await _customerCache.Find(id, token);

        if (customer == null)
        {
            var customerProto = await _customersClient.GetCustomerAsync(new() { Id = (int)id }, cancellationToken: token);
            customer = customerProto?.ToDto();
        }

        if (customer != null)
            await _customerCache.Insert(customer, token);

        return customer;
    }
}
