namespace Ozon.Route256.Practice.OrderService.Application.Models.Dto;

public class CustomerDto
{
    public int Id { get; init; }
    public string FirstName { get; init; }
    public string LastName { get; init; }
    public string MobileNumber { get; init; }
    public string Email { get; init; }
    public AddressDto Address { get; init; }
    public IEnumerable<AddressDto> Addresses { get; init; }

    public CustomerDto(int id,
        string firstName,
        string mobileNumber,
        string email,
        string lastName,
        AddressDto address,
        IEnumerable<AddressDto> addresses)
    {
        Id = id;
        FirstName = firstName;
        LastName = lastName;
        MobileNumber = mobileNumber;
        Email = email;
        Address = address;
        Addresses = addresses;
    }

    public CustomerDto()
    {
    }
}