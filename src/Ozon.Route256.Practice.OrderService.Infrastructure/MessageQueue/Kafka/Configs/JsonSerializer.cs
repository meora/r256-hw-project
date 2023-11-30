using Confluent.Kafka;
using System.Text.Json;

namespace Ozon.Route256.Practice.OrderService.Infrastructure.MessageQueue.Kafka.Configs;

internal class JsonSerializer<T> : ISerializer<T>
{
    public byte[] Serialize(T data, SerializationContext context)
    {
        var options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = false
        };

        return JsonSerializer.SerializeToUtf8Bytes(data, options);
    }
}
