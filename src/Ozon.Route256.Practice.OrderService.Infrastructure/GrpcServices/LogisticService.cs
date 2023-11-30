using Grpc.Core;
using Ozon.Route256.Practice.LogisticsSimulator.Grpc;
using Ozon.Route256.Practice.OrderService.Application.Interfaces;

namespace Ozon.Route256.Practice.OrderService.Infrastructure.GrpcServices;

internal class LogisticService : ILogisticService
{
    private readonly LogisticsSimulatorService.LogisticsSimulatorServiceClient _logisticsClient;

    public LogisticService(LogisticsSimulatorService.LogisticsSimulatorServiceClient logisticsClient)
    {
        _logisticsClient = logisticsClient;
    }

    public async Task<bool> CancelOrder(long id, CancellationToken token)
    {
        token.ThrowIfCancellationRequested();
        var response = await _logisticsClient.OrderCancelAsync(new() { Id = id }); 
        
        if (response.Success is false)
            throw new RpcException(new Status(StatusCode.FailedPrecondition, response.Error));

        return response.Success;
    }
}
