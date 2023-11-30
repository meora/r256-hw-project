using Ozon.Route256.Practice.GatewayService.Models.Dto.Responses;

namespace Ozon.Route256.Practice.GatewayService.GrpcServices
{
    public interface ICustomersService
    {
        Task<GetCustomersResponseDto> GetCustomers(CancellationToken token);
    }
}