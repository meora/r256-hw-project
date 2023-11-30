using Grpc.Core;
using Grpc.Net.Client.Balancer;
using Grpc.Net.Client.Configuration;
using Ozon.Route256.Practice.GatewayService.GrpcServices;
using Ozon.Route256.Practice.GatewayService.Middlewares;
using System.Text.Json.Serialization;

namespace Ozon.Route256.Practice.GatewayService
{
    public class Startup
    {
        private readonly IConfiguration _configuration;

        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {

            services.AddControllers()
                    .AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
            });
            services.Configure<RouteOptions>(options => options.LowercaseUrls = true);
            services.AddSwaggerGen();

            var factory = new StaticResolverFactory(address => new[]
            {
                 new BalancerAddress("host.docker.internal", 3311),
                 new BalancerAddress("host.docker.internal", 3312)
            });

            services.AddSingleton<ResolverFactory>(factory);

            services.AddGrpcClient<Orders.OrdersClient>((provider, options) =>
            {
                options.Address = new Uri("static:///orders-service");
                options.ChannelOptionsActions.Add(o =>
                {
                    o.Credentials = ChannelCredentials.Insecure;
                    o.ServiceConfig = new ServiceConfig
                    {
                        LoadBalancingConfigs = { new RoundRobinConfig() }
                    };
                    o.ServiceProvider = provider;
                });
            });


            services.AddGrpcClient<Regions.RegionsClient>(options =>
            {
                options.Address = new Uri("static:///regions-service");
            })
            .ConfigureChannel(x =>
            {
                x.Credentials = ChannelCredentials.Insecure;
                x.ServiceConfig = new ServiceConfig()
                {
                    LoadBalancingConfigs = { new LoadBalancingConfig("round_robin") }
                };
            });

            services.AddGrpcClient<Customers.CustomersClient>(options =>
            {
                var url = _configuration.GetValue<string>("ROUTE256_CUSTOMER_SERVICE_ADDRES");
                if (string.IsNullOrEmpty(url))
                    throw new Exception("ROUTE256_CUSTOMER_SERVICE_ADDRES variable is empty");

                options.Address = new Uri(url);
            });

            services.AddScoped<IOrdersService, OrdersService>();
            services.AddScoped<IRegionsService, RegionsService>();
            services.AddScoped<ICustomersService, CustomersService>();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseDeveloperExceptionPage();
            app.UseSwagger();
            app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Gateway"));

            app.UseMiddleware<ExceptionMiddleware>();

            app.UseRouting();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
