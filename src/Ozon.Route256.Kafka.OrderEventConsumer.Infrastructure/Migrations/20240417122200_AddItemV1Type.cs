using System;
using FluentMigrator;
using Ozon.Route256.Kafka.OrderEventConsumer.Infrastructure.Common;

namespace Ozon.Route256.Postgres.Persistence.Migrations;

[Migration(20240417122100, TransactionBehavior.None)]
public class AddItemV1Type : SqlMigration
{
    protected override string GetUpSql(IServiceProvider services) => @"
do $$
    begin
        if not exists (select 1 from pg_type where typname = 'item_v1') then
            create type item_v1 as
            (
                  id         bigint
                , item_id    bigint
                , created    bigint
                , delivered  bigint
                , cancelled  bigint
                , created_at timestamp with time zone
            );
        end if;
    end
$$;";

    protected override string GetDownSql(IServiceProvider services) => @"
do $$
    begin
        drop type if exists item_v1;
    end
$$;";
}