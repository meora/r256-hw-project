using Ozon.Route256.Practice.OrderService.Application.Interfaces;
using Ozon.Route256.Practice.OrderService.Domain.Entities;

namespace Ozon.Route256.Practice.OrderService.Infrastructure.Repositories.Impl.InMemory;

internal class StorageInMemoryRepository : IStorageRepository
{
    private readonly InMemoryStorage _inMemoryStorage;

    public StorageInMemoryRepository(InMemoryStorage inMemoryStorage)
    {
        _inMemoryStorage = inMemoryStorage;
    }

    public Task<Storage?> FindByRegion(string region, CancellationToken token)
    {
        if (token.IsCancellationRequested)
            return Task.FromCanceled<Storage?>(token);

        _inMemoryStorage.Storages.TryGetValue(region, out var storage);
        return Task.FromResult(storage);
    }
}
