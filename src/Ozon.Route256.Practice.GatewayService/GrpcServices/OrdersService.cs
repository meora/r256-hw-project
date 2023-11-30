using Ozon.Route256.Practice.GatewayService.Extensions;
using Ozon.Route256.Practice.GatewayService.Models.Dto.Requests;
using Ozon.Route256.Practice.GatewayService.Models.Dto.Responses;

namespace Ozon.Route256.Practice.GatewayService.GrpcServices
{
    public sealed class OrdersService : IOrdersService
    {
        private readonly Orders.OrdersClient _ordersClient;
        private readonly ILogger<OrdersService> _logger;

        public OrdersService(ILogger<OrdersService> logger, Orders.OrdersClient ordersClient)
        {
            _logger = logger;
            _ordersClient = ordersClient;
        }

        public async Task CancelOrder(long id, CancellationToken token)
        {
            await _ordersClient.CancelOrderAsync(new() { Id = id }, cancellationToken: token);
        }

        public async Task<OrderStatusResponseDto> GetOrderStatus(long id, CancellationToken token)
        {
            var response = await _ordersClient.GetOrderStatusAsync(new() { Id = id }, cancellationToken: token);

            return new((Models.Enums.OrderState)response.Status);
        }

        public async Task<GetOrdersResponseDto> GetOrders(GetOrdersRequestDto request, CancellationToken token)
        {
            var response = await _ordersClient.GetOrdersAsync(request.ToProto(), cancellationToken: token);

            return new(response.Orders.Select(x => x.ToDto()));
        }

        public async Task<GetOrdersByRegionResponseDto> GetOrdersByRegion(GetOrdersByRegionRequestDto request, CancellationToken token)
        {
            var response = await _ordersClient.GetOrdersByRegionAsync(request.ToProto(), cancellationToken: token);

            return new(response.OrdersByRegionAggregations.Select(x => x.ToDto()));
        }

        public async Task<GetOrdersByCustomerResponseDto> GetOrdersByCustomer(long id, GetOrdersByCustomerRequestDto request, CancellationToken token)
        {
            var response = await _ordersClient.GetOrdersByCustomerAsync(request.ToProto(id), cancellationToken: token);

            return new(response.Orders.Select(x => x.ToDto()));
        }
    }
}
