using Confluent.Kafka;

namespace Ozon.Route256.Practice.OrderService.Infrastructure.MessageQueue.Kafka.Consumers;

internal interface IConsumerProvider<TValue>
{
    IConsumer<string, TValue> Create(ConsumerConfig config);
}