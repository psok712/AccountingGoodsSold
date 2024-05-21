namespace Kafka.OrderEventConsumer.Infrastructure.OrderProduce;

public sealed record OrderEventPositionProduce(
    long ItemId,
    int Quantity,
    MoneyProduce Price = null!
);