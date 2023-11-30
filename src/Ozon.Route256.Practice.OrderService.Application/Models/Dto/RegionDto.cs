namespace Ozon.Route256.Practice.OrderService.Application.Models.Dto;
public class RegionDto
{
    public long Id { get; init; }
    public string Name { get; init; }

    public RegionDto(long id,
        string name)
    {
        Id = id;
        Name = name;
    }

    public RegionDto()
    {
    }

}