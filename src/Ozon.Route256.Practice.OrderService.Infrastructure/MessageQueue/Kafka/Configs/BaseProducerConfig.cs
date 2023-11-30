using Confluent.Kafka;

namespace Ozon.Route256.Practice.OrderService.Infrastructure.MessageQueue.Kafka.Configs;
internal class BaseProducerConfig
{
    public string Topic { get; init; } = null!;
    public ProducerConfig Config { get; init; } = new();
}
