using System;
using Kafka.OrderEventConsumer.Domain.ValueObjects;

namespace Kafka.OrderEventConsumer.Domain.Order;

public sealed record OrderEvent(
    OrderId OrderId,
    UserId UserId,
    WarehouseId WarehouseId,
    Status Status,
    DateTime Moment,
    OrderEventPosition[] Positions);