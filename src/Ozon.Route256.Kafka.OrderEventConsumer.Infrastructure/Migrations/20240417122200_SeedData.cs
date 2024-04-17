using System;
using FluentMigrator;
using Ozon.Route256.Kafka.OrderEventConsumer.Infrastructure.Common;

namespace Ozon.Route256.Postgres.Persistence.Migrations;

[Migration(20240417122200, TransactionBehavior.None)]
public class SeedData : SqlMigration {
    
    protected override string GetUpSql(IServiceProvider services) => @"
insert into  goods_accounting (id, status, amount, update_at)
values (1, 'Created',   0, null) 
     , (2, 'Cancelled', 0, null)
     , (3, 'Delivered', 0, null)
on conflict (id) do nothing;
";

    protected override string GetDownSql(IServiceProvider services) => @"
delete from goods_accounting
where id in (1, 2, 3);
";
}