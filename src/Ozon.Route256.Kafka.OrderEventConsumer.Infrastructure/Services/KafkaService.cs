using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Ozon.Route256.Kafka.OrderEventConsumer.Domain.Interfaces;
using Ozon.Route256.Kafka.OrderEventConsumer.Domain.Models;
using Ozon.Route256.Kafka.OrderEventConsumer.Domain.Order;
using Ozon.Route256.Kafka.OrderEventConsumer.Domain.ValueObjects;

namespace Ozon.Route256.Kafka.OrderEventConsumer.Infrastructure.Services;

public class KafkaService : IKafkaService
{
    private readonly IItemRepository _itemRepository;

    private readonly ISalesRepository _salesRepository;

    public KafkaService(IItemRepository itemRepository, ISalesRepository salesRepository)
    {
        _itemRepository = itemRepository;
        _salesRepository = salesRepository;
    }

    public async Task OrderProcessing(OrderEvent orderEvent, CancellationToken token)
    {
        await Task.WhenAll(orderEvent.Positions.Select(async position =>
        {
            var itemId = position.ItemId.Value;
            await _itemRepository.AddIfNotExist(itemId, token);

            switch (orderEvent.Status)
            {
                case Status.Created:
                    await _itemRepository.IncCreated(itemId, token);
                    break;

                case Status.Delivered:
                {
                    await _itemRepository.IncDelivered(itemId, token);
                    await _salesRepository.AddIfNotExist(CreateSalesAddModel(position), token);
                    await _salesRepository.IncSale(CreateSalesIncModel(position), token);
                    break;
                }

                case Status.Canceled:
                    await _itemRepository.IncCanceled(itemId, token);
                    break;

                default:
                    throw new ArgumentException("status doesn't exist");
            }
        }));
    }

    private SalesAddModel CreateSalesAddModel(OrderEventPosition orderEvent)
    {
        var sellerId = long.Parse(orderEvent.ItemId.Value.ToString().Substring(0, 6));
        var itemId = long.Parse(orderEvent.ItemId.Value.ToString().Substring(6));

        return new SalesAddModel
        {
            Currency = orderEvent.Price.Currency,
            ItemId = itemId,
            SellerId = sellerId
        };
    }

    private SalesIncModel CreateSalesIncModel(OrderEventPosition orderEvent)
    {
        var sellerId = long.Parse(orderEvent.ItemId.Value.ToString().Substring(0, 6));
        var itemId = long.Parse(orderEvent.ItemId.Value.ToString().Substring(6));

        return new SalesIncModel
        {
            ItemId = itemId,
            SellerId = sellerId,
            Price = orderEvent.Price.Value
        };
    }
}