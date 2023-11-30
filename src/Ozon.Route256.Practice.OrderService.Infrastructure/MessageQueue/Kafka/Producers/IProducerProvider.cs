using Confluent.Kafka;

namespace Ozon.Route256.Practice.OrderService.Infrastructure.MessageQueue.Kafka.Producers;

internal interface IProducerProvider<TMessage>
{
    IProducer<string, TMessage> Get();
}
