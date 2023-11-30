namespace Ozon.Route256.Practice.Shared.Dto;

public record AddressDto(
    string Region,
    string City,
    string Street,
    string Building,
    string Apartment,
    double Latitude,
    double Longitude);