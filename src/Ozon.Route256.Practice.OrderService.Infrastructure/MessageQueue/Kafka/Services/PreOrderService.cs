using Geolocation;
using Microsoft.Extensions.Logging;
using Ozon.Route256.Practice.OrderService.Application.Interfaces;
using Ozon.Route256.Practice.OrderService.Domain.Entities;
using Ozon.Route256.Practice.OrderService.Infrastructure.MessageQueue.Kafka.Models;
using Ozon.Route256.Practice.OrderService.Application.Models.Extenstions;
using Ozon.Route256.Practice.OrderService.Infrastructure.MessageQueue.Kafka.Producers;

namespace Ozon.Route256.Practice.OrderService.Infrastructure.MessageQueue.Kafka.Services;

internal class PreOrderService : IPreOrderService
{
    private const double MAX_DISTANCE = 5000;

    private readonly ILogger<PreOrderService> _logger;
    private readonly NewOrdersProducer _newOrdersProducer;
    private readonly ICustomerCacheService _customerCacheService;
    private readonly IOrderRepository _orderRepository;
    private readonly IStorageRepository _storageRepository;

    public PreOrderService(IOrderRepository orderRepository,
                           NewOrdersProducer newOrdersProducer,
                           ICustomerCacheService customerCacheService,
                           IStorageRepository storageRepository,
                           ILogger<PreOrderService> logger)
    {
        _orderRepository = orderRepository;
        _newOrdersProducer = newOrdersProducer;
        _customerCacheService = customerCacheService;
        _storageRepository = storageRepository;
        _logger = logger;
    }

    public async Task HandlePreOrder(KafkaPreOrder preOrder, CancellationToken token)
    {
        token.ThrowIfCancellationRequested();

        var customer = await _customerCacheService.Find(preOrder.Customer.Id, token);
        if (customer == null)
            throw new Exception($"Customer with id={preOrder.Customer.Id} for pre order not found");

        var order = new Order(
            preOrder.Id,
            preOrder.Goods.Length,
            preOrder.Goods.Sum(x => x.Price),
            preOrder.Goods.Sum(x => x.Weight),
            (Domain.Enums.OrderType)preOrder.Source,
            DateTime.UtcNow,
            preOrder.Customer.Address.Region,
            Domain.Enums.OrderState.Created,
            customer.FirstName + " " + customer.LastName,
            preOrder.Customer.Address.ToDomain(),
            customer.MobileNumber,
            preOrder.Customer.Id
        );

        await _orderRepository.Insert(order, token);
        await ValidateOrder(preOrder, token);

        _logger.LogInformation($"Order with id: {preOrder.Id} is valid");
        await _newOrdersProducer.Produce(order.ToDto(), token);
    }

    private async Task ValidateOrder(KafkaPreOrder preOrder, CancellationToken token)
    {
        var storage = await _storageRepository.FindByRegion(preOrder.Customer.Address.Region, token);
        if (storage == null)
            throw new Exception($"Storage by region {preOrder.Customer.Address.Region} not found");

        Coordinate origin = new Coordinate(storage.Latitude, storage.Longtitude);
        Coordinate destination = new Coordinate(preOrder.Customer.Address.Latitude, preOrder.Customer.Address.Longitude);

        var distance = GeoCalculator.GetDistance(origin, destination, distanceUnit: DistanceUnit.Kilometers);
        if (distance > MAX_DISTANCE)
            throw new Exception($"Distance={distance} value exceeds permissible distance ({MAX_DISTANCE} km)");
    }
}
