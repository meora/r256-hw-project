using FluentMigrator;
using Ozon.Route256.Practice.OrderService.Infrastructure.Dal.Common;

namespace Ozon.Route256.Practice.OrderService.Infrastructure.Dal.Migrations;

[Migration(1, "Initial migration")]
public class Initial : SqlMigration
{
    protected override string GetUpSql(
    IServiceProvider services) => @"
            create type order_type as enum ('WebSite', 'Mobile', 'Api');
            create type order_state as enum ('Created', 'SentToCustomer', 'Delivered', 'Lost', 'Cancelled');

            create table regions(
                id bigserial primary key,
                name text
            );

            insert into regions (""id"", ""name"") values (1, 'Moscow');
            insert into regions (""id"", ""name"") values (2, 'StPetersburg');
            insert into regions (""id"", ""name"") values (3, 'Novosibirsk');

            create table storages(
                id bigserial primary key,
                region_id int8,
                latitude float8,
                longtitude float8
            );

            insert into storages (""region_id"", ""latitude"", ""longtitude"") values (1, 55.73, 37.86);
            insert into storages (""region_id"", ""latitude"", ""longtitude"") values (2, 59.83, 30.45);
            insert into storages (""region_id"", ""latitude"", ""longtitude"") values (3, 54.98, 83.00);

            create table orders(
                id bigserial primary key,
                quantity int,
                total_amount float4,
                total_weight float4,
                order_type order_type,
                order_date timestamp,
                region text,
                order_status order_state,
                client_name text,
                delivery_address jsonb,
                phone_number text,
                customer_id int8
            );";

    protected override string GetDownSql(
        IServiceProvider services) => @"
                drop table regions;
                drop table storages; 
                drop table orders; 

                drop type order_type;
                drop type order_state;
            ";
}
