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

    public async Task AddIfNotExist(long itemId, CancellationToken token)
    {
        const string sqlQuery = @"
insert into items (item_id, created, delivered, canceled, updated_at) 
select @ItemId, 0, 0, 0, @UpdatedAt
    on conflict (item_id) do nothing;
";

        var @params = new DynamicParameters();
        @params.Add("ItemId", itemId);
        @params.Add("UpdatedAt", dateTimeOffsetProvider.UtcNow);

        await using var connection = await dataSource.OpenConnectionAsync(token);
        await connection.QueryAsync<long>(
            new CommandDefinition(sqlQuery, @params, cancellationToken: token));
    }

    public async Task<ItemEntityV1> Get(long itemId, CancellationToken token)
    {
        const string sqlQuery = @"
select *
  from items
 where item_id = @ItemId
";

        var @params = new DynamicParameters();
        @params.Add("ItemId", itemId);


        var cmd = new CommandDefinition(
            sqlQuery,
            @params,
            commandTimeout: DefaultTimeoutInSeconds,
            cancellationToken: token);

        await using var connection = await dataSource.OpenConnectionAsync(token);
        return (await connection.QueryAsync<ItemEntityV1>(cmd)).First();
    }

    public async Task IncCreated(long itemId, CancellationToken token)
    {
        const string sqlQuery = @"
update items
   set created = created + 1
     , updated_at = @UpdateAt
 where item_id = @ItemId
";
        var @params = new DynamicParameters();
        @params.Add("UpdateAt", dateTimeOffsetProvider.UtcNow);
        @params.Add("ItemId", itemId);

        await using var connection = await dataSource.OpenConnectionAsync(token);
        await connection.QueryAsync<long>(
            new CommandDefinition(
                sqlQuery,
                @params,
                cancellationToken: token));
    }

    public async Task IncCanceled(long itemId, CancellationToken token)
    {
        const string sqlQuery = @"
update items
   set created = created - 1
     , canceled = canceled + 1
     , updated_at = @UpdateAt
 where item_id = @ItemId
";
        var @params = new DynamicParameters();
        @params.Add("UpdateAt", dateTimeOffsetProvider.UtcNow);
        @params.Add("ItemId", itemId);

        await using var connection = await dataSource.OpenConnectionAsync(token);
        await connection.QueryAsync<long>(
            new CommandDefinition(
                sqlQuery,
                @params,
                cancellationToken: token));
    }

    public async Task IncDelivered(long itemId, CancellationToken token)
    {
        const string sqlQuery = @"
update items
   set created = created - 1
     , delivered = items.delivered + 1
     , updated_at = @UpdateAt
 where item_id = @ItemId
";
        var @params = new DynamicParameters();
        @params.Add("UpdateAt", dateTimeOffsetProvider.UtcNow);
        @params.Add("ItemId", itemId);

        await using var connection = await dataSource.OpenConnectionAsync(token);
        await connection.QueryAsync<long>(
            new CommandDefinition(
                sqlQuery,
                @params,
                cancellationToken: token));
    }
}