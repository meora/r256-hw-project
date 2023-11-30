using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Ozon.Route256.Practice.OrderService.Application.Interfaces;
using Ozon.Route256.Practice.OrderService.Extensions;

namespace Ozon.Route256.Practice.OrderService.GrpcServices;

public sealed class OrderService : Orders.OrdersBase
{
    private readonly ILogisticService _logisticService;
    private readonly IOrderRepository _orderRepository;
    private readonly IRegionRepository _regionRepository;
    private readonly ICustomerCacheService _customerCacheService;

    public OrderService(ILogisticService logisticService,
                        IOrderRepository orderRepository,
                        IRegionRepository regionRepository,
                        ICustomerCacheService customerCacheService)
    {
        _logisticService = logisticService;
        _orderRepository = orderRepository;
        _regionRepository = regionRepository;
        _customerCacheService = customerCacheService;
    }

    public override async Task<Empty> CancelOrder(CancelOrderByIdRequest request, ServerCallContext context)
    {
        var order = await _orderRepository.Find(request.Id, context.CancellationToken);
        if (order is null)
            throw new RpcException(new Status(StatusCode.NotFound, $"Order with id={request.Id} not found"));
        if (order.IsCanBeCancelled())
            throw new RpcException(new Status(StatusCode.FailedPrecondition, $"Order with id={request.Id} cannot be canceled"));

        var isSuccess = await _logisticService.CancelOrder(request.Id, context.CancellationToken);

        if (isSuccess)
        {
            order.SetStatusCanceled();
            await _orderRepository.Update(order, context.CancellationToken);
        }

        return new();
    }

    public override async Task<OrderStatusResponse> GetOrderStatus(GetOrderStatusByIdRequest request, ServerCallContext context)
    {
        var result = await _orderRepository.Find(request.Id, context.CancellationToken);
        if (result is null)
            throw new RpcException(new Status(StatusCode.NotFound, $"Order with id={request.Id} not found"));

        return new OrderStatusResponse()
        {
            Status = (OrderState)result.OrderStatus
        };
    }

    public override async Task<GetOrdersResponse> GetOrders(GetOrdersRequest request, ServerCallContext context)
    {
        var requestDto = request.ToDto();

        await ValidateRegions(context, requestDto.Regions);

        var orders = await _orderRepository.Get(requestDto, context.CancellationToken);
        if (orders.Any() is false)
            throw new RpcException(new Status(StatusCode.NotFound, $"Orders not found"));

        return new GetOrdersResponse
        {
            Orders = { orders.Select(x => x.ToProto()) }
        };
    }

    public override async Task<GetOrdersByRegionResponse> GetOrdersByRegion(GetOrdersByRegionRequest request, ServerCallContext context)
    {
        var requestDto = request.ToDto();

        await ValidateRegions(context, requestDto.Regions);

        var aggregations = await _orderRepository.GetByRegions(requestDto, context.CancellationToken);
        return new GetOrdersByRegionResponse
        {
            OrdersByRegionAggregations = { aggregations.Select(x => x.ToProto()) }
        };
    }

    public override async Task<GetOrdersByCustomerResponse> GetOrdersByCustomer(GetOrdersByCustomerRequest request, ServerCallContext context)
    {
        var requestDto = request.ToDto();

        var customer = await _customerCacheService.Find(request.CustomerId, context.CancellationToken);

        if (customer is null)
            throw new RpcException(new Status(StatusCode.NotFound, $"User with id={request.CustomerId} not found"));

        var orders = await _orderRepository.GetByCustomer(requestDto, context.CancellationToken);
        if (orders.Any() is false)
            throw new RpcException(new Status(StatusCode.NotFound, $"Orders not found"));

        return new GetOrdersByCustomerResponse()
        {
            Orders = { orders.Select(x => x.ToProto()) }
        };
    }

    private async Task ValidateRegions(ServerCallContext context, string[] requestedRegions)
    {
        if (requestedRegions.Any())
        {
            var regions = await _regionRepository.FindMany(requestedRegions.ToArray(), context.CancellationToken);
            if (!requestedRegions.SequenceEqual(regions.Select(r => r.Name)))
                throw new RpcException(new Status(StatusCode.FailedPrecondition, "One or more regions are invalid."));
        }
    }
}