using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Transactions;
using Ozon.Route256.Kafka.OrderEventConsumer.Domain.Entities;
using Ozon.Route256.Kafka.OrderEventConsumer.Domain.Interfaces;
using Ozon.Route256.Kafka.OrderEventConsumer.Domain.Models;
using Ozon.Route256.Kafka.OrderEventConsumer.Domain.Order;
using Ozon.Route256.Kafka.OrderEventConsumer.Domain.ValueObjects;

namespace Ozon.Route256.Kafka.OrderEventConsumer.Infrastructure.Services;

public class KafkaService(
    IItemRepository itemRepository, 
    ISalesRepository salesRepository
    ) : IKafkaService
{
    public async Task OrderProcessing(OrderEvent orderEvent, CancellationToken token)
    {
        await Task.WhenAll(orderEvent.Positions.Select(async position =>
        {
            var itemId = position.ItemId.Value;
            
            using var transaction = CreateTransactionScope();
            await itemRepository.AddIfNotExist(itemId, token);

            switch (orderEvent.Status)
            {
                case Status.Created:
                    await itemRepository.IncCreated(itemId, token);
                    break;

                case Status.Delivered:
                {
                    await itemRepository.IncDelivered(itemId, token);
                    
                    await salesRepository.AddIfNotExist(CreateSalesAddModel(position), token);
                    await salesRepository.IncSale(CreateSalesEntity(position), token);
                    break;
                }

                case Status.Canceled:
                    await itemRepository.IncCanceled(itemId, token);
                    break;

                default:
                    throw new ArgumentException("status doesn't exist");
            }
            
            transaction.Complete();
        }));
    }

    private SalesAddModel CreateSalesAddModel(OrderEventPosition orderEvent)
    {
        var sellerId = long.Parse(orderEvent.ItemId.Value.ToString()[..6]);
        var itemId = long.Parse(orderEvent.ItemId.Value.ToString()[6..]);

        return new SalesAddModel
        {
            Currency = orderEvent.Price.Currency,
            ItemId = itemId,
            SellerId = sellerId
        };
    }

    private SalesEntityV1 CreateSalesEntity(OrderEventPosition orderEvent)
    {
        var sellerId = long.Parse(orderEvent.ItemId.Value.ToString()[..6]);
        var itemId = long.Parse(orderEvent.ItemId.Value.ToString()[6..]);

        return new SalesEntityV1
        {
            ItemId = itemId,
            SellerId = sellerId,
            Currency = orderEvent.Price.Currency,
            Price = orderEvent.Price.Value
        };
    }

    private static TransactionScope CreateTransactionScope(
        IsolationLevel isolationLevel = IsolationLevel.ReadCommitted)
    {
        var options = new TransactionOptions
        {
            IsolationLevel = isolationLevel,
            Timeout = TimeSpan.FromSeconds(5)
        };
        
        return new TransactionScope(
            TransactionScopeOption.Required, 
            options, 
            TransactionScopeAsyncFlowOption.Enabled);
    }
}