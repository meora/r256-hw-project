using Confluent.Kafka;
using Microsoft.Extensions.Options;
using Ozon.Route256.Practice.OrderService.Infrastructure.MessageQueue.Kafka.Configs;

namespace Ozon.Route256.Practice.OrderService.Infrastructure.MessageQueue.Kafka.Producers;

internal class ProducerProvider<TMessage> : IProducerProvider<TMessage>
{
    private readonly IOptions<NewOrdersProducerConfig> _config;

    private readonly IProducer<string, TMessage> _producer;

    public ProducerProvider(IOptions<NewOrdersProducerConfig> config)
    {
        _config = config;
        _producer = new ProducerBuilder<string, TMessage>(config.Value.Config)
            .SetValueSerializer(new JsonSerializer<TMessage>())
            .Build();
    }

    public IProducer<string, TMessage> Get() =>
        _producer;

    public void Dispose()
    {
        _producer.Dispose();
    }
}
