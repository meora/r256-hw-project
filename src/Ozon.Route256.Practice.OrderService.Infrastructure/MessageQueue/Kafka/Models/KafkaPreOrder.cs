using Ozon.Route256.Practice.OrderService.Application.Models.Dto;

namespace Ozon.Route256.Practice.OrderService.Infrastructure.MessageQueue.Kafka.Models;

internal record KafkaPreOrder(
    long Id,
    int Source,
    CustomerDto Customer,
    GoodDto[] Goods);
