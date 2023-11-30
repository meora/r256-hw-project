using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Ozon.Route256.Practice.OrderService.Infrastructure.MessageQueue.Kafka.Configs;
using Ozon.Route256.Practice.OrderService.Infrastructure.MessageQueue.Kafka.Models;
using Ozon.Route256.Practice.OrderService.Infrastructure.MessageQueue.Kafka.Services;

namespace Ozon.Route256.Practice.OrderService.Infrastructure.MessageQueue.Kafka.Consumers;

internal class PreOrdersConsumer : BaseConsumer<KafkaPreOrder, PreOrdersConsumerConfig>
{
    private readonly IServiceScopeFactory _serviceScopeFactory;

    public PreOrdersConsumer(ILogger<BaseConsumer<KafkaPreOrder, PreOrdersConsumerConfig>> logger,
                        IConsumerProvider<KafkaPreOrder> consumerProvider,
                        IOptions<PreOrdersConsumerConfig> config,
                        IServiceScopeFactory serviceScopeFactory)
        : base(logger, consumerProvider, config)
    {
        _serviceScopeFactory = serviceScopeFactory;
    }

    public override async Task Handle(KafkaPreOrder message, CancellationToken token)
    {
        if (message is null) return;

        using var scope = _serviceScopeFactory.CreateScope();
        var preOrderService = scope.ServiceProvider.GetRequiredService<IPreOrderService>();
        await preOrderService.HandlePreOrder(message, token);
    }
}
