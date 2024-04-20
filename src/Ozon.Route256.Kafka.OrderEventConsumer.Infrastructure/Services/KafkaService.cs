using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
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
            var updateModel = new ItemUpdateModel
            {
                ItemId = position.ItemId.Value,
                Quantity = position.Quantity
            };

            switch (orderEvent.Status)
            {
                case Status.Created:
                    await itemRepository.AddUpdateCreated(updateModel, token);
                    break;

                case Status.Delivered:
                {
                    await itemRepository.AddUpdateDelivered(updateModel, token);
                    
                    await salesRepository.AddUpdateSale(CreateSalesEntity(position), updateModel.Quantity, token);
                    break;
                }

                case Status.Canceled:
                    await itemRepository.AddUpdateCanceled(updateModel, token);
                    break;

                default:
                    throw new ArgumentException("status doesn't exist");
            }
        }));
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
}