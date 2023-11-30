using Confluent.Kafka;
using Microsoft.Extensions.Logging;
using Ozon.Route256.Practice.OrderService.Infrastructure.MessageQueue.Kafka.Configs;

namespace Ozon.Route256.Practice.OrderService.Infrastructure.MessageQueue.Kafka.Consumers;

internal class ConsumerProvider<TMessage> : IConsumerProvider<TMessage>
{
    private readonly ILogger<ConsumerProvider<TMessage>> _logger;

    public ConsumerProvider(
        ILogger<ConsumerProvider<TMessage>> logger)
    {
        _logger = logger;
    }

    public IConsumer<string, TMessage> Create(ConsumerConfig config)
    {
        return new ConsumerBuilder<string, TMessage>(
                config)
            .SetPartitionsAssignedHandler(
                (consumer, topicPartitions) =>
                    _logger.LogInformation(
                        "Partition assigned: {TopicPartitions}",
                        string.Join(
                            Environment.NewLine,
                            topicPartitions
                                .Select(part => $"{part.Topic}: {part.Partition.ToString()}"))))
            .SetKeyDeserializer(Deserializers.Utf8)
            .SetValueDeserializer(new JsonDeserializer<TMessage>())
            .Build();
    }
}