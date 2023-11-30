using Microsoft.AspNetCore.Mvc;
using Ozon.Route256.Practice.GatewayService.GrpcServices;

namespace Ozon.Route256.Practice.GatewayService.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class RegionController : ControllerBase
    {
        private readonly ILogger<RegionController> _logger;
        private readonly IRegionsService _regionsService;

        public RegionController(ILogger<RegionController> logger, IRegionsService regionsService)
        {
            _logger = logger;
            _regionsService = regionsService;
        }

        [HttpGet]
        public async Task<IActionResult> GetRegions(CancellationToken token)
        {
            return Ok(await _regionsService.GetRegions(token));
        }
    }
}
