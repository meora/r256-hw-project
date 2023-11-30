using Microsoft.Extensions.Logging;
using Ozon.Route256.Practice.OrderService.Application.Interfaces;
using Ozon.Route256.Practice.OrderService.Application.Models.Enums;
using Ozon.Route256.Practice.OrderService.Infrastructure.MessageQueue.Kafka.Models;

namespace Ozon.Route256.Practice.OrderService.Infrastructure.MessageQueue.Kafka.Services;

internal class OrderEventService : IOrderEventService
{
    private readonly ILogger<OrderEventService> _logger;
    private readonly IOrderRepository _orderRepository;

    public OrderEventService(ILogger<OrderEventService> logger, IOrderRepository orderRepository)
    {
        _logger = logger;
        _orderRepository = orderRepository;
    }

    public async Task HandleOrderEvent(KafkaOrderEvent orderEvent, CancellationToken token)
    {
        token.ThrowIfCancellationRequested();

        var order = await _orderRepository.Find(orderEvent.Id, token);
        if (order == null)
            throw new Exception($"Order with id={orderEvent.Id} not found");

        _logger.LogInformation($"Order with id {order.Id} state changed from {order.OrderStatus} to {(OrderState)orderEvent.NewState}");

        order.SetStatus((Domain.Enums.OrderState)orderEvent.NewState);
        await _orderRepository.Update(order, token);
    }
}
