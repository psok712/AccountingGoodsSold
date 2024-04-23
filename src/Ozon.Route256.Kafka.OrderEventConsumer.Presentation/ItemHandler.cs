using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Confluent.Kafka;
using Microsoft.Extensions.Logging;
using Ozon.Route256.Kafka.OrderEventConsumer.Domain.Enums;
using Ozon.Route256.Kafka.OrderEventConsumer.Domain.Interfaces;
using Ozon.Route256.Kafka.OrderEventConsumer.Domain.Order;
using Ozon.Route256.Kafka.OrderEventConsumer.Domain.ValueObjects;
using Ozon.Route256.Kafka.OrderEventConsumer.Infrastructure.Kafka;
using Ozon.Route256.Kafka.OrderEventConsumer.Infrastructure.OrderProduce;

namespace Ozon.Route256.Kafka.OrderEventConsumer.Presentation;

public class ItemHandler : IHandler<long, OrderEventProduce>
{
    private readonly ILogger<ItemHandler> _logger;
    private readonly IKafkaService _service;

    public ItemHandler(ILogger<ItemHandler> logger, IKafkaService service)
    {
        _logger = logger;
        _service = service;
    }

    public async Task Handle(IReadOnlyCollection<ConsumeResult<long, OrderEventProduce>> messages,
        CancellationToken token)
    {
        var tasks = messages
            .Select(async m =>
                await _service.OrderProcessing(OrderEventProduceToOrderEvent(m.Message.Value), token))
            .ToList();
        await Task.WhenAll(tasks);

        _logger.LogInformation("Handled {Count} messages", messages.Count);
    }

    private OrderEvent OrderEventProduceToOrderEvent(OrderEventProduce orderEventProduce)
    {
        var positions = orderEventProduce.Positions
            .Select(p =>
                new OrderEventPosition(CreateItemId(p.ItemId), p.Quantity, CreateMoney(p.Price)))
            .ToArray();

        return new OrderEvent(
            CreateOrderId(orderEventProduce.OrderId),
            CreateUserId(orderEventProduce.UserId),
            CreateWarehouse(orderEventProduce.WarehouseId),
            CreateStatus(orderEventProduce.Status),
            orderEventProduce.Moment,
            positions
        );
    }

    private OrderId CreateOrderId(long orderId)
    {
        return new OrderId(orderId);
    }

    private UserId CreateUserId(long userId)
    {
        return new UserId(userId);
    }

    private ItemId CreateItemId(long itemId)
    {
        return new ItemId(itemId);
    }

    private WarehouseId CreateWarehouse(long warehouseId)
    {
        return new WarehouseId(warehouseId);
    }

    private Money CreateMoney(MoneyProduce money)
    {
        const long mathFraction = 1_000_000_000;
        return new Money
        {
            Value = money.Units + (decimal)money.Nanos / mathFraction,
            Currency = money.Currency
        };
    }

    private Status CreateStatus(OrderStatus orderStatus)
    {
        return (Status)orderStatus;
    }
}