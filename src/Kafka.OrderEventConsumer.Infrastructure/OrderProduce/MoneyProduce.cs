namespace Kafka.OrderEventConsumer.Infrastructure.OrderProduce;

public sealed record MoneyProduce(
    long Units,
    int Nanos,
    string Currency = null!);