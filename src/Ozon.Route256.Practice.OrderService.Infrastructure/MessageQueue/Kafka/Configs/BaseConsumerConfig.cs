using Confluent.Kafka;

namespace Ozon.Route256.Practice.OrderService.Infrastructure.MessageQueue.Kafka.Configs;
internal class BaseConsumerConfig
{
    public string Topic { get; init; } = null!;
    public ConsumerConfig Config { get; init; } = new();
}