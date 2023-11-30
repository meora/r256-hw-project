using StackExchange.Redis;

namespace Ozon.Route256.Practice.OrderService.Infrastructure.Cache.Redis;

internal interface IRedisDatabaseFactory
{
    IDatabase GetDatabase();
    IServer GetServer();
}
