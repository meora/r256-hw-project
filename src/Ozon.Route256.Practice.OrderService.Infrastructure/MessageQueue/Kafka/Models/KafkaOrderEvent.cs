namespace Ozon.Route256.Practice.OrderService.Infrastructure.MessageQueue.Kafka.Models;

internal record KafkaOrderEvent(long Id, int NewState, DateTime UpdateDate);
