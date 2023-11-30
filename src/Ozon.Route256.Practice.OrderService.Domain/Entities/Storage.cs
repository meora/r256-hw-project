namespace Ozon.Route256.Practice.OrderService.Domain.Entities;

public class Storage
{
    public long Id { get; init; }
    public long RegionId { get; init; }
    public double Latitude { get; init; }
    public double Longtitude { get; init; }

    public Storage(long id,
        long regionId,
        double latitude,
        double longtitude)
    {
        Id = id;
        RegionId = regionId;
        Latitude = latitude;
        Longtitude = longtitude;
    }

    public Storage()
    {
    }
}
