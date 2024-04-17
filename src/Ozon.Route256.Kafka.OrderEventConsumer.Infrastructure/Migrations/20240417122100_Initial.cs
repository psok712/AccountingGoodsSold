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
";

    protected override string GetDownSql(IServiceProvider services) => @"
drop table if exists items;
";
}
