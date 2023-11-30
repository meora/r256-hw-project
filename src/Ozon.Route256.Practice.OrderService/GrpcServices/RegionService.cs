using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Ozon.Route256.Practice.OrderService.Application.Interfaces;

namespace Ozon.Route256.Practice.OrderService.GrpcServices;

public class RegionService : Regions.RegionsBase
{
    private readonly IRegionRepository _regionRepository;

    public RegionService(IRegionRepository regionRepository)
    {
        _regionRepository = regionRepository;
    }

    public override async Task<GetRegionsResponse> GetRegions(Empty request, ServerCallContext context)
    {
        var regions = await _regionRepository.Get(context.CancellationToken);
        return new()
        {
            Regions = { regions.Select(x => x.Name) }
        };
    }
}
