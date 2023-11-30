namespace Ozon.Route256.Practice.Shared.Extensions
{
    public static class ProtoExtenstions
    {
        public static GetOrdersByCustomerRequest SetId(this GetOrdersByCustomerRequest proto, long customerId)
        {
            proto.CustomerId = customerId;
            return proto;
        }
    }
}
