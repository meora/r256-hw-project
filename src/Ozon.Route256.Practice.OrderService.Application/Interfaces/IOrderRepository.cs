using Ozon.Route256.Practice.OrderService.Application.Models.Requests;
using Ozon.Route256.Practice.OrderService.Domain.Aggregates;
using Ozon.Route256.Practice.OrderService.Domain.Entities;

namespace Ozon.Route256.Practice.OrderService.Application.Interfaces;

public interface IOrderRepository
{
    Task<Order?> Find(long id, CancellationToken token);
    Task<Order[]> Get(GetOrdersRequestDto request, CancellationToken token);
    Task<Order[]> GetByCustomer(GetOrdersByCustomerRequestDto request, CancellationToken token);
    Task<OrderAggregate[]> GetByRegions(GetOrdersByRegionRequestDto request, CancellationToken token);
    Task Update(Order order, CancellationToken token);
    Task<long> Insert(Order order, CancellationToken token);
}
