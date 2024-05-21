using System;
using Kafka.OrderEventConsumer.Domain.Enums;

namespace Kafka.OrderEventConsumer.Infrastructure.OrderProduce;

public sealed record OrderEventProduce(
    long OrderId,
    long UserId,
    long WarehouseId,
    OrderStatus Status,
    DateTime Moment,
    OrderEventPositionProduce[] Positions);