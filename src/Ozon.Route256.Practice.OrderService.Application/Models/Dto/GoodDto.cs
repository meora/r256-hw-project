namespace Ozon.Route256.Practice.OrderService.Application.Models.Dto;

public class GoodDto
{
    public long Id { get; init; }
    public string Name { get; init; }
    public int Quantity { get; init; }
    public float Price { get; init; }
    public long Weight { get; init; }

    public GoodDto(long id,
        string name,
        int quantity,
        float price,
        long weight)
    {
        Id = id;
        Name = name;
        Quantity = quantity;
        Price = price;
        Weight = weight;
    }

    public GoodDto()
    {
    }
}