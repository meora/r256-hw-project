using Ozon.Route256.Practice.OrderService.Application.Models.Requests;
using Ozon.Route256.Practice.OrderService.Application.Models.Dto;
using Ozon.Route256.Practice.OrderService.Application.Models.Enums;
using Ozon.Route256.Practice.OrderService.Infrastructure.Extensions;
using Ozon.Route256.Practice.OrderService.Application.Interfaces;
using Ozon.Route256.Practice.OrderService.Domain.Entities;
using Ozon.Route256.Practice.OrderService.Domain.Aggregates;

namespace Ozon.Route256.Practice.OrderService.Infrastructure.Repositories.Impl.InMemory;

internal class OrderInMemoryRepository : IOrderRepository
{
    private readonly InMemoryStorage _inMemoryStorage;

    public OrderInMemoryRepository(InMemoryStorage inMemoryStorage)
    {
        _inMemoryStorage = inMemoryStorage;
    }

    public Task<Order?> Find(long id, CancellationToken token)
    {
        if (token.IsCancellationRequested)
            return Task.FromCanceled<Order?>(token);

        var order = _inMemoryStorage.Orders.GetValueOrDefault(id);

        return Task.FromResult(order);
    }

    public Task<Order[]> Get(GetOrdersRequestDto request, CancellationToken token)
    {
        if (token.IsCancellationRequested)
            return Task.FromCanceled<Order[]>(token);

        var query = _inMemoryStorage.Orders.Values.AsQueryable();

        if (request.Regions.Any())
            query = query.Where(x => request.Regions.Contains(x.Region));
        ;
        if (request.OrderType != default)
            query = query.Where(x => x.OrderType == (Domain.Enums.OrderType)request.OrderType);

        if (request.SortField is not null)
        {
            if (request.SortDirection == SortDirection.DESC)
                query = query.OrderByDescending(request.SortField.Value.ToString());
            else
                query = query.OrderBy(request.SortField.Value.ToString());
        }

        return Task.FromResult(query
            .Skip(request.PageNumber * request.PageSize)
            .Take(request.PageSize)
            .ToArray());
    }

    public Task<Order[]> GetByCustomer(GetOrdersByCustomerRequestDto request, CancellationToken token)
    {
        if (token.IsCancellationRequested)
            return Task.FromCanceled<Order[]>(token);

        var query = _inMemoryStorage.Orders.Values.Where(x => x.CustomerId == request.CustomerId);

        if (request.StartDateTime != default)
            query = query.Where(x => x.OrderDate >= request.StartDateTime);

        return Task.FromResult(query
            .Skip(request.PageNumber * request.PageSize)
            .Take(request.PageSize)
            .ToArray());
    }

    public Task<OrderAggregate[]> GetByRegions(GetOrdersByRegionRequestDto request, CancellationToken token)
    {
        if (token.IsCancellationRequested)
            return Task.FromCanceled<OrderAggregate[]>(token);

        var query = _inMemoryStorage.Orders.Values.AsQueryable();

        if (request.Regions.Any())
            query = query.Where(x => request.Regions.Contains(x.Region));

        if (request.StartDateTime != default)
            query = query.Where(x => x.OrderDate >= request.StartDateTime);

        var result = query
            .GroupBy(x => x.Region, (key, list) => new OrderAggregate(
                key,
                list.Count(),
                list.Sum(o => o.TotalAmount),
                list.Sum(o => o.TotalWeight),
                list.DistinctBy(c => c.CustomerId).Count()))
            .ToArray();

        return Task.FromResult(result);
    }

    public Task Update(Order order, CancellationToken token)
    {
        if (token.IsCancellationRequested)
            return Task.FromCanceled(token);

        if (!_inMemoryStorage.Orders.ContainsKey(order.Id))
            throw new Exception($"Order with id={order.Id} not found");

        _inMemoryStorage.Orders[order.Id] = order;

        return Task.CompletedTask;
    }

    public Task<long> Insert(Order order, CancellationToken token)
    {
        if (token.IsCancellationRequested)
            return Task.FromCanceled<long>(token);

        _inMemoryStorage.Orders[order.Id] = order;

        return Task.FromResult(order.Id);
    }
}
