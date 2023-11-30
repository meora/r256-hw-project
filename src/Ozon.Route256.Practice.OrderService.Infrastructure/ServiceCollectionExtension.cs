using Dapper;
using FluentMigrator.Runner;
using FluentMigrator.Runner.Processors;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Ozon.Route256.Practice.OrderService.Application.Interfaces;
using Ozon.Route256.Practice.OrderService.Application.Models.Dto;
using Ozon.Route256.Practice.OrderService.Infrastructure.Cache.Redis.Repositories;
using Ozon.Route256.Practice.OrderService.Infrastructure.Cache.Redis.Services;
using Ozon.Route256.Practice.OrderService.Infrastructure.Cache.Redis;
using Ozon.Route256.Practice.OrderService.Infrastructure.ClientBalancing;
using Ozon.Route256.Practice.OrderService.Infrastructure.Dal.Common;
using Ozon.Route256.Practice.OrderService.Infrastructure.Dal.Common.Shard;
using Ozon.Route256.Practice.OrderService.Infrastructure.MessageQueue.Kafka.Services;
using Ozon.Route256.Practice.OrderService.Infrastructure.Repositories.Impl.InMemory;
using Ozon.Route256.Practice.OrderService.Infrastructure.Repositories.Impl.Shard;
using Ozon.Route256.Practice.OrderService.Infrastructure.MessageQueue.Kafka.Configs;
using Ozon.Route256.Practice.OrderService.Infrastructure.MessageQueue.Kafka.Consumers;
using Ozon.Route256.Practice.OrderService.Infrastructure.MessageQueue.Kafka.Producers;
using Ozon.Route256.Practice.LogisticsSimulator.Grpc;
using Ozon.Route256.Practice.OrderService.Infrastructure.Interceptors;

namespace Ozon.Route256.Practice.OrderService.Infrastructure;

public static partial class ServiceCollectionExtension
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection collection, IConfiguration configuration)
    {
        collection.AddGrpcServices(configuration);
        collection.AddDAL(configuration);
        collection.AddKafka();
        collection.AddRedis(configuration);

        return collection;
    }

    public static IServiceCollection AddDAL(this IServiceCollection collection, IConfiguration configuration)
    {

        var connectionString = configuration.GetConnectionString("OrderDb");
        collection.AddSingleton<IPostgresConnectionFactory>(_ => new PostgresConnectionFactory(connectionString));

        collection.AddSingleton<IDbStore, DbStore>();
        collection.Configure<DbOptions>(configuration.GetSection(nameof(DbOptions)));
        collection.AddSingleton<IShardConnectionFactory, ShardConnectionFactory>();
        collection.AddSingleton<IShardingRule<long>, LongShardingRule>();
        collection.AddSingleton<IShardingRule<string>, StringShardingRule>();
        collection.AddSingleton<IShardMigrator, ShardMigrator>();

        collection.AddFluentMigratorCore()
            .ConfigureRunner(
                builder => builder
                    .AddPostgres()
                    .ScanIn(typeof(SqlMigration).Assembly)
                    .For.Migrations())
            .AddOptions<ProcessorOptions>()
            .Configure(
                options =>
                {
                    options.ConnectionString = connectionString;
                    options.Timeout = TimeSpan.FromSeconds(30);
                });

        SqlMapper.AddTypeHandler(new DateTimeHandler());
        SqlMapper.AddTypeHandler(new JsonDataTypeHandler<AddressDto>());

        collection.AddSingleton<InMemoryStorage>();

        collection.AddScoped<IOrderRepository, OrderShardRepository>();
        collection.AddScoped<IRegionRepository, RegionShardRepository>();
        collection.AddScoped<IStorageRepository, StorageShardRepository>();

        return collection;
    }

    public static IServiceCollection AddKafka(this IServiceCollection collection)
    {
        collection
                .AddOptions<PreOrdersConsumerConfig>()
                .Configure<IConfiguration>(
                    (opt, config) =>
                        config
                            .GetSection("Kafka:Consumers:PreOrders")
                            .Bind(opt));

        collection
            .AddOptions<OrdersEventsConsumerConfig>()
            .Configure<IConfiguration>(
                (opt, config) =>
                    config
                        .GetSection("Kafka:Consumers:OrdersEvents")
                        .Bind(opt));

        collection
            .AddOptions<NewOrdersProducerConfig>()
            .Configure<IConfiguration>(
                (opt, config) =>
                    config
                        .GetSection("Kafka:Producer:NewOrders")
                        .Bind(opt));

        collection.AddSingleton(typeof(IConsumerProvider<>), typeof(ConsumerProvider<>))
            .AddHostedService<PreOrdersConsumer>()
            .AddHostedService<OrdersEventsConsumer>();

        collection.AddSingleton(typeof(IProducerProvider<>), typeof(ProducerProvider<>))
            .AddScoped<NewOrdersProducer>();

        collection.AddScoped<IPreOrderService, PreOrderService>();
        collection.AddScoped<IOrderEventService, OrderEventService>();

        return collection;
    }

    public static IServiceCollection AddRedis(this IServiceCollection collection, IConfiguration configuration)
    {
        var connectionString = configuration.GetSection("Redis")
            .GetValue<string>("ConnectionString");

        collection.AddScoped<IRedisDatabaseFactory>(_ =>
            new RedisDatabaseFactory(connectionString));

        collection.AddScoped<ICustomerCacheRepository, CustomerCacheRedisRepository>();
        collection.AddScoped<ICustomerCacheService, CustomerCacheService>();

        return collection;
    }

    public static IServiceCollection AddGrpcServices(this IServiceCollection collection, IConfiguration configuration)
    {
        collection.AddSingleton<IDbStore, DbStore>();

        collection.AddGrpcClient<SdService.SdServiceClient>(options =>
        {
            var url = configuration.GetValue<string>("ROUTE256_SD_ADDRESS");
            if (string.IsNullOrEmpty(url))
                throw new Exception("ROUTE256_SD_ADDRESS variable is empty");

            options.Address = new Uri(url);
        });

        collection.AddGrpcClient<LogisticsSimulatorService.LogisticsSimulatorServiceClient>(options =>
        {
            var url = configuration.GetValue<string>("ROUTE256_LS_ADDRESS");
            if (string.IsNullOrEmpty(url))
                throw new Exception("ROUTE256_LS_ADDRESS variable is empty");

            options.Address = new Uri(url);
        });

        collection.AddGrpcClient<Customers.CustomersClient>(options =>
        {
            var url = configuration.GetValue<string>("ROUTE256_CUSTOMER_SERVICE_ADDRESS");
            if (string.IsNullOrEmpty(url))
                throw new Exception("ROUTE256_CUSTOMER_SERVICE_ADDRESS variable is empty");

            options.Address = new Uri(url);
        });

        collection.AddHostedService<SdConsumerHostedService>();
        collection.AddGrpcReflection();

        return collection;
    }
}
