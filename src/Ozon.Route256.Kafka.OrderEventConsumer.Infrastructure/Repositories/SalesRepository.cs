using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Dapper;
using Npgsql;
using Ozon.Route256.Kafka.OrderEventConsumer.Domain.Entities;
using Ozon.Route256.Kafka.OrderEventConsumer.Domain.Interfaces;
using Ozon.Route256.Kafka.OrderEventConsumer.Domain.Models;

namespace Ozon.Route256.Kafka.OrderEventConsumer.Infrastructure.Repositories;

public class SalesRepository(NpgsqlDataSource dataSource) : ISalesRepository
{
    private const int DefaultTimeoutInSeconds = 5;

    public async Task AddUpdateSale(SalesEntityV1 sale, long quantity, CancellationToken token)
    {
        const string sqlQuery = @"
insert into sales (seller_id, item_id, currency, sales) 
select @SellerId, @ItemId, @Currency, @Price * @Quantity
    on conflict (seller_id, item_id, currency)
    do update set 
       sales = sales.sales + @Price * @Quantity
";
        
        var @params = new DynamicParameters();
        @params.Add("SellerId", sale.SellerId);
        @params.Add("ItemId", sale.ItemId);
        @params.Add("Currency", sale.Currency);
        @params.Add("Price", sale.Price);
        @params.Add("Quantity", quantity);

        await using var connection = await dataSource.OpenConnectionAsync(token);
        await connection.QueryAsync<long>(
            new CommandDefinition(sqlQuery, @params, cancellationToken: token));
    }

    public async Task<SalesEntityV1> Get(SalesGetModel sale, CancellationToken token)
    {
        const string sqlQuery = @"
select *
  from sales
 where seller_id = @SellerId
   and item_id = @ItemId
";

        var @params = new DynamicParameters();
        @params.Add("SellerId", sale.SellerId);
        @params.Add("ItemId", sale.ItemId);

        var cmd = new CommandDefinition(
            sqlQuery,
            @params,
            commandTimeout: DefaultTimeoutInSeconds,
            cancellationToken: token);

        await using var connection = await dataSource.OpenConnectionAsync(token);
        return (await connection.QueryAsync<SalesEntityV1>(cmd)).First();
    }
}