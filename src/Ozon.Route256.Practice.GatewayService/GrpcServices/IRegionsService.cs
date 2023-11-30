using Ozon.Route256.Practice.GatewayService.Models.Dto.Responses;

namespace Ozon.Route256.Practice.GatewayService.GrpcServices
{
    public interface IRegionsService
    {
        Task<GetRegionsResponseDto> GetRegions(CancellationToken token);
    }
}
