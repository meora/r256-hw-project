using Ozon.Route256.Practice.OrderService.Domain.Entities;
using System.Collections.Concurrent;

namespace Ozon.Route256.Practice.OrderService.Infrastructure.Repositories.Impl.InMemory;

internal class InMemoryStorage
{
    public readonly ConcurrentDictionary<string, Region> Regions = new(2, 10);
    public readonly ConcurrentDictionary<string, Storage> Storages = new(2, 10);
    public readonly ConcurrentDictionary<long, Order> Orders = new(2, 10);

    public InMemoryStorage()
    {
        FakeOrders();
        FakeRegions();
        FakeStorages();
    }

    private void FakeOrders()
    {
        var faker = new Bogus.Faker();
        var orders = Enumerable.Range(1, 100)
            .Select(x =>
            {
                return new Order(
                    x,
                    faker.Random.Number(1, 10),
                    faker.Random.Number(1000, 10000),
                    faker.Random.Number(1, 10),
                    faker.Random.Enum<Domain.Enums.OrderType>(),
                    DateTime.SpecifyKind(faker.Date.Recent(), DateTimeKind.Utc),
                    faker.Random.CollectionItem(Regions).Key,
                    faker.Random.Enum<Domain.Enums.OrderState>(),
                    faker.Name.FullName(),
                    new Domain.Entities.Address(
                        faker.Random.CollectionItem(Regions).Key,
                        faker.Address.City(),
                        faker.Address.StreetName(),
                        faker.Address.BuildingNumber(),
                        faker.Random.Number(1, 100).ToString(),
                        faker.Random.Double(55, 56),
                        faker.Random.Double(35, 85)
                    ),
                    faker.Phone.PhoneNumber(),
                    faker.Random.Number(1, 100)
                );
            });

        foreach (var order in orders)
            Orders[order.Id] = order;
    }

    private void FakeRegions()
    {
        Regions["Moscow"] = new(1, "Moscow");
        Regions["StPetersburg"] = new(2, "StPetersburg");
        Regions["Novosibirsk"] = new(3, "Novosibirsk");
    }

    private void FakeStorages()
    {
        Storages["Moscow"] = new(1, 1, 55.73, 37.86);
        Storages["StPetersburg"] = new(2, 2, 59.83, 30.45);
        Storages["Novosibirsk"] = new(3, 3, 54.98, 83.00);
    }
}
