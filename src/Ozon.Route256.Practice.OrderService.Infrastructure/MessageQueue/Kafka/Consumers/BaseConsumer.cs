using Confluent.Kafka;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Ozon.Route256.Practice.OrderService.Infrastructure.MessageQueue.Kafka.Configs;

namespace Ozon.Route256.Practice.OrderService.Infrastructure.MessageQueue.Kafka.Consumers;

internal abstract class BaseConsumer<TMessage, TOption> : BackgroundService
    where TMessage : class
    where TOption : BaseConsumerConfig
{
    private readonly ILogger<BaseConsumer<TMessage, TOption>> _logger;
    private readonly IConsumerProvider<TMessage> _consumerProvider;
    private readonly IOptions<TOption> _config;

    public BaseConsumer(
        ILogger<BaseConsumer<TMessage, TOption>> logger,
        IConsumerProvider<TMessage> consumerProvider,
        IOptions<TOption> config)
    {
        _logger = logger;
        _consumerProvider = consumerProvider;
        _config = config;
    }

    protected override async Task ExecuteAsync(CancellationToken token)
    {
        while (token.IsCancellationRequested is false)
        {
            using var consumer = _consumerProvider
                .Create(_config.Value.Config);

            try
            {
                consumer.Subscribe(_config.Value.Topic);

                await ConsumeCycle(consumer, token);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Consumer error");

                try
                {
                    consumer.Unsubscribe();
                }
                catch
                {
                    // ignored
                }

                await Task.Delay(TimeSpan.FromSeconds(1), token);
            }
        }
    }

    private async Task ConsumeCycle(IConsumer<string, TMessage> consumer, CancellationToken token)
    {
        while (token.IsCancellationRequested is false)
        {
            var consumeResult = consumer.Consume(token);

            _logger.LogInformation(
                "{@Topic}:{@Partition}:{@Offset} => Consumed event with key = {@Key} value = {@Value}",
                consumeResult.Topic, consumeResult.Partition, consumeResult.Offset, consumeResult.Message.Key, consumeResult.Message.Value);

            await Handle(consumeResult.Value, token);
        }
    }

    public abstract Task Handle(TMessage message, CancellationToken token);
}
