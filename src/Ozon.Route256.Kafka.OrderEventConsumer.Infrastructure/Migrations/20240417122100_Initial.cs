using System;
using FluentMigrator;

using Ozon.Route256.Kafka.OrderEventConsumer.Infrastructure.Common;

namespace Ozon.Route256.Postgres.Persistence.Migrations;

[Migration(20240417122100, TransactionBehavior.None)]
public sealed class Initial : SqlMigration
{
    protected override string GetUpSql(IServiceProvider services) => @"
create table if not exists goods_accounting 
(   id                     bigserial   not null 
  , status                 text        not null
  , amount                 bigint      not null
  , update_at              timestamptz null
  , CONSTRAINT statuses_pk primary key (id));
";

    protected override string GetDownSql(IServiceProvider services) => @"
drop table if exists goods_accounting;
";
}
