using Ozon.Route256.Practice.GatewayService.Models.Dto.Requests;
using Ozon.Route256.Practice.GatewayService.Models.Dto.Responses;

namespace Ozon.Route256.Practice.GatewayService.GrpcServices
{
    public interface IOrdersService
    {
        Task CancelOrder(long id, CancellationToken token);
        Task<GetOrdersResponseDto> GetOrders(GetOrdersRequestDto request, CancellationToken token);
        Task<GetOrdersByCustomerResponseDto> GetOrdersByCustomer(long id, GetOrdersByCustomerRequestDto request, CancellationToken token);
        Task<GetOrdersByRegionResponseDto> GetOrdersByRegion(GetOrdersByRegionRequestDto request, CancellationToken token);
        Task<OrderStatusResponseDto> GetOrderStatus(long id, CancellationToken token);
    }
}