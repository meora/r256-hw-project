using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Ozon.Route256.Practice.OrderService.Infrastructure.MessageQueue.Kafka.Configs;
using Ozon.Route256.Practice.OrderService.Infrastructure.MessageQueue.Kafka.Models;
using Ozon.Route256.Practice.OrderService.Infrastructure.MessageQueue.Kafka.Services;

namespace Ozon.Route256.Practice.OrderService.Infrastructure.MessageQueue.Kafka.Consumers;

internal class OrdersEventsConsumer : BaseConsumer<KafkaOrderEvent, OrdersEventsConsumerConfig>
{
    private readonly IServiceScopeFactory _serviceScopeFactory;

    public OrdersEventsConsumer(ILogger<BaseConsumer<KafkaOrderEvent, OrdersEventsConsumerConfig>> logger,
                        IConsumerProvider<KafkaOrderEvent> consumerProvider,
                        IOptions<OrdersEventsConsumerConfig> config,
                        IServiceScopeFactory serviceScopeFactory)
        : base(logger, consumerProvider, config)
    {
        _serviceScopeFactory = serviceScopeFactory;
    }

    public override async Task Handle(KafkaOrderEvent message, CancellationToken token)
    {
        if (message is null) return;

        using var scope = _serviceScopeFactory.CreateScope();
        var orderEventService = scope.ServiceProvider.GetRequiredService<IOrderEventService>();
        await orderEventService.HandleOrderEvent(message, token);
    }
}
