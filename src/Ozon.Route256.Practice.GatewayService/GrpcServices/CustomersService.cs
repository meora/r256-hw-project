using Ozon.Route256.Practice.GatewayService.Extensions;
using Ozon.Route256.Practice.GatewayService.Models.Dto.Responses;

namespace Ozon.Route256.Practice.GatewayService.GrpcServices
{
    public sealed class CustomersService : ICustomersService
    {
        private readonly Customers.CustomersClient _customersClient;
        private readonly ILogger<CustomersService> _logger;

        public CustomersService(ILogger<CustomersService> logger, Customers.CustomersClient customersClient)
        {
            _logger = logger;
            _customersClient = customersClient;
        }

        public async Task<GetCustomersResponseDto> GetCustomers(CancellationToken token)
        {
            var response = await _customersClient.GetCustomersAsync(new(), cancellationToken: token);

            return new(response.Customers.Select(x => x.ToDto()).ToArray());
        }
    }
}
