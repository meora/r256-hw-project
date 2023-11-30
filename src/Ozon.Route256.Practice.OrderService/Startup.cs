using Ozon.Route256.Practice.OrderService.GrpcServices;
using Ozon.Route256.Practice.OrderService.Infrastructure;
using Ozon.Route256.Practice.OrderService.Interceptors;

namespace Ozon.Route256.Practice.OrderService;

public class Startup
{
    private readonly IConfiguration _configuration;

    public Startup(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public void ConfigureServices(IServiceCollection services)
    {
        services.AddGrpc(x => x.Interceptors.Add<LoggerInterceptor>());
        services.AddInfrastructure(_configuration);
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        app.UseRouting();
        app.UseEndpoints(endpoints =>
        {
            endpoints.MapGrpcService<GrpcServices.OrderService>();
            endpoints.MapGrpcService<RegionService>();
            endpoints.MapGrpcReflectionService();
        });
    }
}