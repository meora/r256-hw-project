namespace Ozon.Route256.Practice.OrderService.Domain.Entities;

public class Customer
{
    public int Id { get; }
    public string FirstName { get; }
    public string LastName { get; }
    public string MobileNumber { get; }
    public string Email { get; }
    public Address DefaultAddress { get; }
    public IEnumerable<Address> Addresses { get; }

    public Customer(int id,
        string firstName,
        string lastName,
        string mobileNumber,
        string email,
        Address defaultAddress,
        IEnumerable<Address> addresses)
    {
        Id = id;
        FirstName = firstName;
        LastName = lastName;
        MobileNumber = mobileNumber;
        Email = email;
        DefaultAddress = defaultAddress;
        Addresses = addresses;
    }

    public Customer()
    {
    }
}
