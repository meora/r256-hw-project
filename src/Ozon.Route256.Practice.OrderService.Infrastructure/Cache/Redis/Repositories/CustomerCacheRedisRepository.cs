using Ozon.Route256.Practice.OrderService.Application.Interfaces;
using StackExchange.Redis;
using System.Text.Json;

namespace Ozon.Route256.Practice.OrderService.Infrastructure.Cache.Redis.Repositories;

internal class CustomerCacheRedisRepository : ICustomerCacheRepository
{
    private readonly IDatabase _redisDatabase;
    private readonly JsonSerializerOptions _jsonSerializerOptions = new();

    public CustomerCacheRedisRepository(IRedisDatabaseFactory redisDatabaseFactory)
    {
        _redisDatabase = redisDatabaseFactory.GetDatabase();
    }

    public async Task<Domain.Entities.Customer?> Find(long id, CancellationToken token)
    {
        token.ThrowIfCancellationRequested();

        var value = await _redisDatabase
            .StringGetAsync(GetKey(id))
            .WaitAsync(token);

        return ToDto(value);
    }

    public async Task Insert(Domain.Entities.Customer customer, CancellationToken token)
    {
        token.ThrowIfCancellationRequested();

        var redisValue = ToRedisValue(customer);
        await _redisDatabase
            .StringSetAsync(
                GetKey(customer.Id),
                redisValue)
            .WaitAsync(token);
    }

    private static string GetKey(long customerId) =>
        $"customer:{customerId}";

    private RedisValue ToRedisValue(Domain.Entities.Customer customer) =>
        JsonSerializer.Serialize(customer, _jsonSerializerOptions);

    private Domain.Entities.Customer? ToDto(RedisValue redisValue)
    {
        if (string.IsNullOrWhiteSpace(redisValue))
        {
            return null;
        }

        return JsonSerializer.Deserialize<Domain.Entities.Customer>(redisValue, _jsonSerializerOptions);
    }
}
