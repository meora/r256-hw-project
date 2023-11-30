using Ozon.Route256.Practice.OrderService.Infrastructure.MessageQueue.Kafka.Models;

namespace Ozon.Route256.Practice.OrderService.Infrastructure.MessageQueue.Kafka.Services;

internal interface IOrderEventService
{
    Task HandleOrderEvent(KafkaOrderEvent orderEvent, CancellationToken token);
}
