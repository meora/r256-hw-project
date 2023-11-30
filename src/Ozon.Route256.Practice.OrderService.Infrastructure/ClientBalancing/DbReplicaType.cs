namespace Ozon.Route256.Practice.OrderService.Infrastructure.ClientBalancing;

internal enum DbReplicaType
{
    Master = 0,
    Sync = 1,
    Async = 2
}