using Microsoft.AspNetCore.Mvc;
using Ozon.Route256.Practice.GatewayService.GrpcServices;

namespace Ozon.Route256.Practice.GatewayService.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CustomerController : ControllerBase
    {
        private readonly ILogger<CustomerController> _logger;
        private readonly ICustomersService _customersService;

        public CustomerController(ILogger<CustomerController> logger, ICustomersService customersService)
        {
            _logger = logger;
            _customersService = customersService;
        }

        [HttpGet]
        public async Task<IActionResult> Get(CancellationToken token)
        {
            return Ok(await _customersService.GetCustomers(token));
        }
    }
}