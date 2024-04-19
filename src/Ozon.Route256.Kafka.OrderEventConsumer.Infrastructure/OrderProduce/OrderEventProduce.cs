using System;
using Ozon.Route256.Kafka.OrderEventConsumer.Domain.Enums;

namespace Ozon.Route256.Kafka.OrderEventConsumer.Infrastructure.OrderProduce;

public sealed record OrderEventProduce(
    long OrderId,
    long UserId,
    long WarehouseId,
    OrderStatus Status,
    DateTime Moment,
    OrderEventPositionProduce[] Positions);