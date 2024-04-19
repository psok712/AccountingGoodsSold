using System;
using System.Threading;
using System.Threading.Tasks;
using Ozon.Route256.Kafka.OrderEventConsumer.Domain.Enums;
using Ozon.Route256.Kafka.OrderEventConsumer.Domain.Interfaces;
using Ozon.Route256.Kafka.OrderEventConsumer.Domain.OrderProduce;

namespace Ozon.Route256.Kafka.OrderEventConsumer.Infrastructure.Services;

public class ItemService : IItemService
{
    private readonly IItemRepository _repository;

    public ItemService(IItemRepository repository)
    {
        _repository = repository;
    }

    public async Task OrderProcessing(OrderEventProduce orderEvent, CancellationToken token)
    {
        var itemId = orderEvent.OrderId;
        await _repository.AddIfNotExist(itemId, token);
        switch (orderEvent.Status)
        {
            case OrderStatus.Created:
                await _repository.IncCreated(itemId, token);
                break;
            case OrderStatus.Delivered:
                await _repository.IncDelivered(itemId, token);
                break;
            case OrderStatus.Canceled:
                await _repository.IncCanceled(itemId, token);
                break;
            default:
                throw new ArgumentException("status doesn't exists");
        }
    }
}