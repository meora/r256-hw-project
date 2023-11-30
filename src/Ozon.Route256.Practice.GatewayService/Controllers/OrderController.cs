using Microsoft.AspNetCore.Mvc;
using Ozon.Route256.Practice.GatewayService.GrpcServices;
using Ozon.Route256.Practice.GatewayService.Models.Dto.Requests;

namespace Ozon.Route256.Practice.GatewayService.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class OrderController : ControllerBase
    {
        private readonly ILogger<OrderController> _logger;
        private readonly IOrdersService _ordersService;

        public OrderController(ILogger<OrderController> logger, IOrdersService ordersService)
        {
            _logger = logger;
            _ordersService = ordersService;
        }

        [HttpPost("{id}/cancel")]
        public async Task<IActionResult> CancelOrder(long id, CancellationToken token)
        {
            await _ordersService.CancelOrder(id, token);

            return Ok();
        }

        [HttpGet("{id}/status")]
        public async Task<IActionResult> GetStatus(long id, CancellationToken token)
        {
            return Ok(await _ordersService.GetOrderStatus(id, token));
        }

        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] GetOrdersRequestDto request, CancellationToken token)
        {
            return Ok(await _ordersService.GetOrders(request, token));
        }

        [HttpGet("aggregate/region")]
        public async Task<IActionResult> GetByRegion([FromQuery] GetOrdersByRegionRequestDto request, CancellationToken token)
        {
            return Ok(await _ordersService.GetOrdersByRegion(request, token));
        }

        [HttpGet("customers/{id}/orders")]
        public async Task<IActionResult> GetOrders(long id, [FromQuery] GetOrdersByCustomerRequestDto request, CancellationToken token)
        {
            return Ok(await _ordersService.GetOrdersByCustomer(id, request, token));
        }
    }
}
