using Kafka.OrderEventConsumer.Domain.ValueObjects;

namespace Kafka.OrderEventConsumer.Domain.Order;

public sealed record OrderEventPosition(ItemId ItemId, int Quantity, Money Price);