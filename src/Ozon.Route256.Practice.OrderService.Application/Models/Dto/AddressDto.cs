namespace Ozon.Route256.Practice.OrderService.Application.Models.Dto;

public class AddressDto
{
    public string Region { get; init; }
    public string City { get; init; }
    public string Street { get; init; }
    public string Building { get; init; }
    public string Apartment { get; init; }
    public double Latitude { get; init; }
    public double Longitude { get; init; }

    public AddressDto(string region,
        string city,
        string street,
        string building,
        string apartment,
        double latitude,
        double longitude)
    {
        Region = region;
        City = city;
        Street = street;
        Building = building;
        Apartment = apartment;
        Latitude = latitude;
        Longitude = longitude;
    }

    public AddressDto()
    {
    }
}