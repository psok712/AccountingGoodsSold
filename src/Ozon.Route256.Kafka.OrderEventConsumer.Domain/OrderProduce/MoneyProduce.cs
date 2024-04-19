namespace Ozon.Route256.Kafka.OrderEventConsumer.Domain.OrderProduce;

public sealed record MoneyProduce(
    long Units,
    int Nanos,
    string Currency = null!);