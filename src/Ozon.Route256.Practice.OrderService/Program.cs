using Microsoft.AspNetCore.Server.Kestrel.Core;
using Ozon.Route256.Practice.OrderService.Infrastructure.Dal.Common.Shard;
using System.Net;

namespace Ozon.Route256.Practice.OrderService;

public static class Program
{
    public static async Task Main(string[] args)
    {
        await CreateHostBuilder(args)
            .Build()
            .RunWithMigrate(args);
    }

    private static IHostBuilder CreateHostBuilder(
        string[] args) =>
        Host.CreateDefaultBuilder(args)
            .ConfigureWebHostDefaults(
                x => x.UseStartup<Startup>()
                    .ConfigureKestrel(
                        options =>
                        {
                            var grpcPort = int.Parse(Environment.GetEnvironmentVariable("ROUTE256_GRPC_PORT")!);
                            var httpPort = int.Parse(Environment.GetEnvironmentVariable("ROUTE256_HTTP_PORT")!);

                            options.Listen(
                                IPAddress.Any,
                                grpcPort,
                                listenOptions => listenOptions.Protocols = HttpProtocols.Http2);

                            options.Listen(
                                IPAddress.Any,
                                httpPort,
                                listenOptions => listenOptions.Protocols = HttpProtocols.Http1);
                        }));

    static async Task RunWithMigrate(
        this IHost host,
        string[] args)
    {
        var needMigration = Environment.GetEnvironmentVariable("Migrate");
        if (needMigration != null && needMigration.Equals("true"))
        {
            using var cts = new CancellationTokenSource(TimeSpan.FromMinutes(1));
            using var scope = host.Services.CreateScope();
            var runner = scope.ServiceProvider.GetRequiredService<IShardMigrator>();
            await runner.Migrate(cts.Token);
        }
        else
        {
            await host.RunAsync();
        }
    }
}
