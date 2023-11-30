using Dapper;
using Ozon.Route256.Practice.OrderService.Application.Interfaces;
using Ozon.Route256.Practice.OrderService.Application.Models.Requests;
using Ozon.Route256.Practice.OrderService.Domain.Aggregates;
using Ozon.Route256.Practice.OrderService.Domain.Entities;
using Ozon.Route256.Practice.OrderService.Infrastructure.Dal.Common.Shard;
using System.Text;
using System.Text.Json;

namespace Ozon.Route256.Practice.OrderService.Infrastructure.Repositories.Impl.Shard;

internal class OrderShardRepository : BaseShardRepository, IOrderRepository
{
    private const string Table = $"{Shards.BucketPlaceholder}.orders";
    private const string OrderTypeCast = $"{Shards.BucketPlaceholder}.order_type";
    private const string OrderStateCast = $"{Shards.BucketPlaceholder}.order_state";
    private const string FieldsForInsert = "id, quantity, total_amount, total_weight, order_type, order_date, region, order_status, client_name, delivery_address, phone_number, customer_id";
    private const string Fields = "id as \"Id\"," +
        "quantity as \"Quantity\", " +
        "total_amount as \"TotalAmount\", " +
        "total_weight as \"TotalWeight\", " +
        "order_type as \"OrderType\", " +
        "order_date as \"OrderDate\", " +
        "region as \"Region\", " +
        "order_status as \"OrderStatus\", " +
        "client_name as \"ClientName\", " +
        "delivery_address as \"DeliveryAddress\", " +
        "phone_number as \"PhoneNumber\", " +
        "customer_id as \"CustomerId\"";

    private readonly IDictionary<string, string> _sortFields = new Dictionary<string, string>()
    {
        { "Id", "id" },
        { "Quantity", "quantity" },
        { "TotalAmount", "total_amount" },
        { "OrderDate", "order_date" }
    };

    public OrderShardRepository(
        IShardConnectionFactory connectionFactory,
        IShardingRule<long> longShardingRule,
        IShardingRule<string> stringShardingRule) : base(connectionFactory, longShardingRule, stringShardingRule)
    {
    }

    public async Task<Order?> Find(long id, CancellationToken token)
    {
        token.ThrowIfCancellationRequested();

        const string sql = @$"
                select {Fields}
                from {Table}
                where id = @id;
            ";

        using var connection = GetConnectionByShardKey(id);
        var result = await connection.QueryAsync<Order>(sql, new { id });

        return result.FirstOrDefault();
    }

    public async Task<Order[]> Get(GetOrdersRequestDto request, CancellationToken token)
    {
        const string sql = @$"
                select {Fields}
                from {Table}
                where true
            ";

        var queryBuilder = new StringBuilder(sql);

        if (request.Regions.Any())
        {
            queryBuilder.AppendLine("and region = any(@regions)");
        }

        if (request.OrderType != default)
        {
            queryBuilder.AppendLine($"and order_type = @orderType::{OrderTypeCast}");
        }

        if (request.SortField.HasValue && _sortFields.TryGetValue(request.SortField.ToString()!, out string sortField))
        {
            queryBuilder.AppendLine($"order by {sortField} {request.SortDirection}");
        }

        queryBuilder.AppendLine("offset @pageNumber * @pageSize limit @pageSize");

        var result = new List<Order>();

        foreach (var bucketId in _connectionFactory.GetAllBuckets())
        {
            using var connection = await GetConnectionByBucket(bucketId, token);

            var orders = await connection.QueryAsync<Order>(queryBuilder.ToString(), new
            {
                regions = request.Regions,
                orderType = request.OrderType.ToString(),
                pageNumber = request.PageNumber,
                pageSize = request.PageSize
            });
            result.AddRange(orders);
        }

        return result.ToArray();
    }

    public async Task<Order[]> GetByCustomer(GetOrdersByCustomerRequestDto request, CancellationToken token)
    {
        token.ThrowIfCancellationRequested();

        const string sql = @$"
                select {Fields}
                from {Table}
                where customer_id = @customerId
                order by order_date
                offset @pageNumber * @pageSize limit @pageSize;
            ";

        var result = new List<Order>();

        foreach (var bucketId in _connectionFactory.GetAllBuckets())
        {
            using var connection = await GetConnectionByBucket(bucketId, token);

            var orders = await connection.QueryAsync<Order>(sql, new
            {
                customerId = request.CustomerId,
                pageNumber = request.PageNumber,
                pageSize = request.PageSize
            });
            result.AddRange(orders);
        }

        return result
            .OrderBy(x => x.OrderDate)
            .Skip(request.PageNumber * request.PageSize)
            .Take(request.PageSize)
            .ToArray();
    }

    public async Task<OrderAggregate[]> GetByRegions(GetOrdersByRegionRequestDto request, CancellationToken token)
    {
        token.ThrowIfCancellationRequested();

        const string sql = @$"
                select
                    region as ""Region"",
                    count(*) as ""OrdersQuantity"",
                    sum(total_amount) as ""TotalAmount"",
                    sum(total_weight) as ""TotalWeight"",
                    count(distinct customer_id) as ""CustomersQuantity""
                from
                    {Table}
                where region = any(@regions)
                group by
                    region
            ";

        var result = new List<OrderAggregate>();

        foreach (var bucketId in _connectionFactory.GetAllBuckets())
        {
            using var connection = await GetConnectionByBucket(bucketId, token);

            var ordersAggregations = await connection.QueryAsync<OrderAggregate>(sql, new
            {
                regions = request.Regions
            });
            result.AddRange(ordersAggregations);
        }

        return result.GroupBy(x => x.Region)
            .Select(g => new OrderAggregate(
                region: g.Key,
                ordersQuantity: g.Sum(x => x.OrdersQuantity),
                totalAmount: g.Sum(x => x.TotalAmount),
                totalWeight: g.Sum(x => x.TotalWeight),
                customersQuantity: g.Sum(x => x.CustomersQuantity))
            ).ToArray();
    }

    public async Task Update(Order order, CancellationToken token)
    {
        token.ThrowIfCancellationRequested();

        const string sql = @$"
                update {Table}
                set
                    quantity = @quantity, 
                    total_amount = @total_amount, 
                    total_weight = @total_weight, 
                    order_type = @order_type::{OrderTypeCast}, 
                    order_date = @order_date, 
                    region = @region, 
                    order_status = @order_status::{OrderStateCast}, 
                    client_name = @client_name, 
                    delivery_address = @delivery_address::jsonb, 
                    phone_number = @phone_number, 
                    customer_id = @customer_id
                where id = @id;
            ";

        using var connection = GetConnectionByShardKey(order.Id);
        var result = await connection.QueryAsync(sql, new
        {
            quantity = order.Quantity,
            total_amount = order.TotalAmount,
            total_weight = order.TotalWeight,
            order_type = order.OrderType.ToString(),
            order_date = order.OrderDate,
            region = order.Region,
            order_status = order.OrderStatus.ToString(),
            client_name = order.ClientName,
            delivery_address = order.DeliveryAddress,
            phone_number = order.PhoneNumber,
            customer_id = order.CustomerId
        });
    }

    public async Task<long> Insert(Order order, CancellationToken token)
    {
        token.ThrowIfCancellationRequested();

        const string sql = @$"
                insert into {Table} ({FieldsForInsert})
                values (
                    @id,
                    @quantity, 
                    @total_amount, 
                    @total_weight, 
                    @order_type::{OrderTypeCast}, 
                    @order_date, 
                    @region, 
                    @order_status::{OrderStateCast}, 
                    @client_name, 
                    @delivery_address::jsonb, 
                    @phone_number, 
                    @customer_id)
                returning id;
            ";

        using var connection = GetConnectionByShardKey(order.Id);
        var result = await connection.QueryAsync<long>(sql, new
        {
            id = order.Id,
            quantity = order.Quantity,
            total_amount = order.TotalAmount,
            total_weight = order.TotalWeight,
            order_type = order.OrderType.ToString(),
            order_date = order.OrderDate,
            region = order.Region,
            order_status = order.OrderStatus.ToString(),
            client_name = order.ClientName,
            delivery_address = JsonSerializer.Serialize(order.DeliveryAddress),
            phone_number = order.PhoneNumber,
            customer_id = order.CustomerId
        });

        return result.FirstOrDefault();
    }
}
