namespace Ozon.Route256.Practice.OrderService.Domain.Entities;

public class Region
{
    public long Id { get; }
    public string Name { get; }

    public Region(long id,
        string name)
    {
        Id = id;
        Name = name;
    }

    public Region()
    { 
    }
}
