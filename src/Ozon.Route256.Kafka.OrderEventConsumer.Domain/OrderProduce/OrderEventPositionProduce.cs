namespace Ozon.Route256.Kafka.OrderEventConsumer.Domain.OrderProduce;

public sealed record OrderEventPositionProduce(
    long ItemId,
    int Quantity,
    MoneyProduce Price = null!
);