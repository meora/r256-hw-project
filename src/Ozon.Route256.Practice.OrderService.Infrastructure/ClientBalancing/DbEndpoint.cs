namespace Ozon.Route256.Practice.OrderService.Infrastructure.ClientBalancing;

internal record DbEndpoint(
    string HostAndPort,
    DbReplicaType DbReplica,
    uint[] Buckets);

