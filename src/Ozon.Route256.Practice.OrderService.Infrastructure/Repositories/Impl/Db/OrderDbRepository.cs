using Dapper;
using Ozon.Route256.Practice.OrderService.Application.Interfaces;
using Ozon.Route256.Practice.OrderService.Application.Models.Requests;
using Ozon.Route256.Practice.OrderService.Domain.Aggregates;
using Ozon.Route256.Practice.OrderService.Domain.Entities;
using Ozon.Route256.Practice.OrderService.Infrastructure.Dal.Common;
using System.Text;
using System.Text.Json;

namespace Ozon.Route256.Practice.OrderService.Infrastructure.Repositories.Impl.Db;

internal class OrderDbRepository : IOrderRepository
{
    private readonly IPostgresConnectionFactory _connectionFactory;

    private const string Table = "orders";
    private const string FieldsForInsert = "quantity, total_amount, total_weight, order_type, order_date, region, order_status, client_name, delivery_address, phone_number, customer_id";
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

    public OrderDbRepository(IPostgresConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public async Task<Order?> Find(long id, CancellationToken token)
    {
        token.ThrowIfCancellationRequested();

        const string sql = @$"
                select {Fields}
                from {Table}
                where id = @id;
            ";

        using var connection = _connectionFactory.GetConnection();
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

        using var connection = _connectionFactory.GetConnection();

        if (request.Regions.Any())
        {
            queryBuilder.AppendLine("and region = any(@regions)");
        }

        if (request.OrderType != default)
        {
            queryBuilder.AppendLine($"and order_type = @orderType::order_type");
        }

        if (request.SortField.HasValue && _sortFields.TryGetValue(request.SortField.ToString()!, out string sortField))
        {
            queryBuilder.AppendLine($"order by {sortField} {request.SortDirection}");
        }

        queryBuilder.AppendLine("offset @pageNumber * @pageSize limit @pageSize");

        var result = await connection.QueryAsync<Order>(queryBuilder.ToString(), new
        {
            regions = request.Regions,
            orderType = request.OrderType.ToString(),
            pageNumber = request.PageNumber,
            pageSize = request.PageSize
        });

        return result.ToArray();
    }

    public async Task<Order[]> GetByCustomer(GetOrdersByCustomerRequestDto request, CancellationToken token)
    {
        token.ThrowIfCancellationRequested();

        const string sql = @$"
                select {Fields}
                from {Table}
                where customer_id = @customerId
                offset @pageNumber * @pageSize limit @pageSize;
            ";

        using var connection = _connectionFactory.GetConnection();
        var result = await connection.QueryAsync<Order>(sql, new
        {
            customerId = request.CustomerId,
            pageNumber = request.PageNumber,
            pageSize = request.PageSize
        });
        return result.ToArray();
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

        using var connection = _connectionFactory.GetConnection();
        var result = await connection.QueryAsync<OrderAggregate>(sql, new
        {
            regions = request.Regions
        });

        return result.ToArray();
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
                    order_type = @order_type::order_type, 
                    order_date = @order_date, 
                    region = @region, 
                    order_status = @order_status::order_state, 
                    client_name = @client_name, 
                    delivery_address = @delivery_address::jsonb, 
                    phone_number = @phone_number, 
                    customer_id = @customer_id
                where id = @id;
            ";

        using var connection = _connectionFactory.GetConnection();
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
                    @quantity, 
                    @total_amount, 
                    @total_weight, 
                    @order_type::order_type, 
                    @order_date, 
                    @region, 
                    @order_status::order_state, 
                    @client_name, 
                    @delivery_address::jsonb, 
                    @phone_number, 
                    @customer_id)
                returning id;
            ";

        using var connection = _connectionFactory.GetConnection();
        var result = await connection.QueryAsync<long>(sql, new
        {
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
