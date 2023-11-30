using FluentMigrator;
using Ozon.Route256.Practice.OrderService.Infrastructure.Dal.Common;

namespace Ozon.Route256.Practice.OrderService.Infrastructure.Dal.Migrations;

[Migration(2, "Add CustomerId Index")]
public class AddCustomerIdIndex : SqlMigration
{
    protected override string GetUpSql(
    IServiceProvider services) => @"
                create index orders_customer_id_idx on orders (customer_id);
            ";

    protected override string GetDownSql(
        IServiceProvider services) => @"
                drop index orders_customer_id_idx;
            ";
}