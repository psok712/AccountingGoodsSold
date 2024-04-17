using System;
using FluentMigrator;

using Ozon.Route256.Kafka.OrderEventConsumer.Infrastructure.Common;

namespace Ozon.Route256.Postgres.Persistence.Migrations;

[Migration(20240417122100, TransactionBehavior.None)]
public sealed class Initial : SqlMigration
{
    protected override string GetUpSql(IServiceProvider services) => @"
create table if not exists items 
(   item_id    bigint      not null
  , created    bigint      not null
  , delivered  bigint      not null
  , cancelled  bigint      not null
  , updated_at timestamp   not null
  , constraint statuses_pk primary key (item_id));
";

    protected override string GetDownSql(IServiceProvider services) => @"
drop table if exists items;
";
}
