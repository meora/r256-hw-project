using Confluent.Kafka;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Ozon.Route256.Practice.OrderService.Application.Models.Dto;
using Ozon.Route256.Practice.OrderService.Infrastructure.MessageQueue.Kafka.Configs;
using Ozon.Route256.Practice.OrderService.Infrastructure.MessageQueue.Kafka.Models;

namespace Ozon.Route256.Practice.OrderService.Infrastructure.MessageQueue.Kafka.Producers;

internal partial class NewOrdersProducer
{
    private readonly ILogger<NewOrdersProducer> _logger;
    private readonly IProducerProvider<NewOrder> _producerProvider;
    private readonly IOptions<NewOrdersProducerConfig> _config;

    public NewOrdersProducer(
        IProducerProvider<NewOrder> producerProvider,
        IOptions<NewOrdersProducerConfig> config,
        ILogger<NewOrdersProducer> logger)
    {
        _producerProvider = producerProvider;
        _config = config;
        _logger = logger;
    }

    public async Task Produce(OrderDto order, CancellationToken token)
    {
        var producer = _producerProvider.Get();
        var kafkaMessage = ToKafka(order);

        _logger.LogInformation($"Order with id: {order.Id} has been produced to Topic: {_config.Value.Topic}");
        await producer.ProduceAsync(_config.Value.Topic, kafkaMessage, token);
    }

    private static Message<string, NewOrder> ToKafka(OrderDto order)
    {
        var newOrder = new NewOrder(order.Id);

        return new Message<string, NewOrder>
        {
            Key = order.Id.ToString(),
            Value = newOrder
        };
    }
}
