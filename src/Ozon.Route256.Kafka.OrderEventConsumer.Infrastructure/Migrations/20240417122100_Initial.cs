using System;
using FluentMigrator;

using Ozon.Route256.Kafka.OrderEventConsumer.Infrastructure.Common;

namespace Ozon.Route256.Postgres.Persistence.Migrations;

[Migration(20240417122100, TransactionBehavior.None)]
public sealed class Initial : SqlMigration
{
    protected override string GetUpSql(IServiceProvider services) => @"
create table if not exists items 
(   item_id    bigint      not null primary key
  , created    bigint      not null
  , delivered  bigint      not null
  , canceled   bigint      not null
  , updated_at timestamp   not null);

create table if not exists sales
(
      seller_id           bigint not null
    , item_id             bigint not null
    , currency            text not null
    , sales               decimal not null
    , constraint pk_sales primary key (seller_id, item_id, currency)
);
";

    protected override string GetDownSql(IServiceProvider services) => @"
drop table if exists items;
drop table if exists sales;
";
}
