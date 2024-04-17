using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Dapper;
using Npgsql;
using Ozon.Route256.Kafka.OrderEventConsumer.Domain.Entities;
using Ozon.Route256.Kafka.OrderEventConsumer.Domain.Interfaces;
using Utils.Providers.Interfaces;

namespace Ozon.Route256.Kafka.OrderEventConsumer.Infrastructure.Repositories;

public sealed class ItemRepository(
    NpgsqlDataSource dataSource,
    IDateTimeOffsetProvider dateTimeOffsetProvider
)
    : IItemRepository
{
    private const int DefaultTimeoutInSeconds = 5;

    public async Task Add(ItemEntityV1 item, CancellationToken token)
    {
        const string sqlQuery = @"
insert into items (item_id, created, delivered, canceled, updated_at) 
select item_id, created, delivered, canceled, updated_at
  from UNNEST(@Item);
";

        await using var connection = await dataSource.OpenConnectionAsync(token);
        await connection.QueryAsync<long>(
            new CommandDefinition(
                sqlQuery,
                item,
                cancellationToken: token));
    }

    public async Task<ItemEntityV1> Get(long itemId, CancellationToken token)
    {
        const string sqlQuery = @"
select *
  from items
where item_id = @ItemId
";

        var @params = new DynamicParameters();
        @params.Add("TaskId", itemId);


        var cmd = new CommandDefinition(
            sqlQuery,
            itemId,
            commandTimeout: DefaultTimeoutInSeconds,
            cancellationToken: token);

        await using var connection = await dataSource.OpenConnectionAsync(token);
        return (await connection.QueryAsync<ItemEntityV1>(cmd)).First();
    }

    public async Task Update(ItemEntityV1 updateModel, CancellationToken token)
    {
        const string sqlQuery = @"
update items
set created = @Created, canceled = @Canceled, delivered = @Delivered, updated_at = @UpdatedAt
where item_id = @ItemId;
";

        var @params = new DynamicParameters();
        @params.Add("ItemId", updateModel.ItemId);
        @params.Add("Created", updateModel.Created);
        @params.Add("Canceled", updateModel.Canceled);
        @params.Add("Delivered", updateModel.Delivered);
        @params.Add("Delivered", dateTimeOffsetProvider.UtcNow);

        await using var connection = await dataSource.OpenConnectionAsync(token);
        await connection.ExecuteAsync(sqlQuery, @params);
    }
}