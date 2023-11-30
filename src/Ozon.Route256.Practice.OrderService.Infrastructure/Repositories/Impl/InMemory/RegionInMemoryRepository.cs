using Ozon.Route256.Practice.OrderService.Application.Interfaces;
using Ozon.Route256.Practice.OrderService.Domain.Entities;

namespace Ozon.Route256.Practice.OrderService.Infrastructure.Repositories.Impl.InMemory;

internal class RegionInMemoryRepository : IRegionRepository
{
    private readonly InMemoryStorage _inMemoryStorage;

    public RegionInMemoryRepository(InMemoryStorage inMemoryStorage)
    {
        _inMemoryStorage = inMemoryStorage ?? throw new ArgumentNullException(nameof(inMemoryStorage));
    }

    public Task<Region[]> Get(CancellationToken token)
    {
        if (token.IsCancellationRequested)
            return Task.FromCanceled<Region[]>(token);

        return Task.FromResult(_inMemoryStorage.Regions.Values.ToArray());
    }

    public Task<Region[]?> FindMany(string[] regions, CancellationToken token)
    {
        if (token.IsCancellationRequested)
            return Task.FromCanceled<Region[]?>(token);

        var result = Find(regions, token).ToArray();
        return Task.FromResult(result);
    }

    private IEnumerable<Region> Find(IEnumerable<string> regions, CancellationToken token)
    {
        foreach (var r in regions)
        {
            token.ThrowIfCancellationRequested();

            if (_inMemoryStorage.Regions.TryGetValue(r, out var region))
                yield return region;
        }
    }
}
