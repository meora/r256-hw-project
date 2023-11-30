using Ozon.Route256.Practice.OrderService.Infrastructure.MessageQueue.Kafka.Models;

namespace Ozon.Route256.Practice.OrderService.Infrastructure.MessageQueue.Kafka.Services;

internal interface IPreOrderService
{
    Task HandlePreOrder(KafkaPreOrder preOrder, CancellationToken token);
}
