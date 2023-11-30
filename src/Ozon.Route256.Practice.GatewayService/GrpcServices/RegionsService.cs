using Ozon.Route256.Practice.GatewayService.Models.Dto.Responses;

namespace Ozon.Route256.Practice.GatewayService.GrpcServices
{
    public class RegionsService : IRegionsService
    {
        private readonly Regions.RegionsClient _regionsClient;

        public RegionsService(Regions.RegionsClient regionsClient)
        {
            _regionsClient = regionsClient ?? throw new ArgumentNullException(nameof(_regionsClient));
        }

        public async Task<GetRegionsResponseDto> GetRegions(CancellationToken token)
        {
            var response = await _regionsClient.GetRegionsAsync(new(), cancellationToken: token);

            return new(response.Regions.ToArray());
        }
    }
}
