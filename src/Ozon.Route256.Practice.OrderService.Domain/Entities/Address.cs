namespace Ozon.Route256.Practice.OrderService.Domain.Entities;

public class Address
{
    public string Region { get; }
    public string City { get; }
    public string Street { get; }
    public string Building { get; }
    public string Apartment { get; }
    public double Latitude { get; }
    public double Longitude { get; }

    public Address(string region,
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

    public Address()
    {
    }
}
