namespace Ozon.Route256.Practice.OrderService.Application.Interfaces;

public interface ILogisticService
{
    Task<bool> CancelOrder(long id, CancellationToken token);
}
