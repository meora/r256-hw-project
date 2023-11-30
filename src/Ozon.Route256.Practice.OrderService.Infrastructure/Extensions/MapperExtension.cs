using Ozon.Route256.Practice.OrderService.Application.Models.Dto;
using Ozon.Route256.Practice.OrderService.Application.Models.Enums;
using Ozon.Route256.Practice.OrderService.Infrastructure.MessageQueue.Kafka.Models;

namespace Ozon.Route256.Practice.OrderService.Infrastructure.Extensions;

internal static class MapperExtension
{
    public static Domain.Entities.Customer ToDto(this Customer proto) =>
        new(
            proto.Id,
            proto.FirstName,
            proto.LastName,
            proto.MobileNumber,
            proto.Email,
            proto.DefaultAddress.ToDto(),
            proto.Addresses.Select(x => x.ToDto())
        ); 
    
    public static Domain.Entities.Address ToDto(this Address proto) =>
        new(
            proto.Region,
            proto.City,
            proto.Street,
            proto.Building,
            proto.Apartment,
            proto.Longitude,
            proto.Latitude
        ); 
    
    public static OrderDto ToDto(this KafkaPreOrder preOrder) =>
        new(
            preOrder.Id,
            preOrder.Goods.Length,
            preOrder.Goods.Sum(x => x.Price),
            preOrder.Goods.Sum(x => x.Weight),
            (OrderType)preOrder.Source,
            DateTime.UtcNow, 
            preOrder.Customer.Address.Region,
            OrderState.Created,
            string.Empty,
            preOrder.Customer.Address,
            string.Empty,
            preOrder.Customer.Id
        );
}
