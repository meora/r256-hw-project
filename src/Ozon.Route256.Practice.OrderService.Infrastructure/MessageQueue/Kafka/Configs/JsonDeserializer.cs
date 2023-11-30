using Confluent.Kafka;
using System.Text.Json;

namespace Ozon.Route256.Practice.OrderService.Infrastructure.MessageQueue.Kafka.Configs;

internal class JsonDeserializer<T> : IDeserializer<T>
{
    public T Deserialize(ReadOnlySpan<byte> data, bool isNull, SerializationContext context)
    {
        if (isNull) return default;

        var utf8Reader = new Utf8JsonReader(data);
        var options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };

        return JsonSerializer.Deserialize<T>(ref utf8Reader, options);
    }
}