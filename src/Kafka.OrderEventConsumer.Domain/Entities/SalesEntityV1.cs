namespace Kafka.OrderEventConsumer.Domain.Entities;

public record SalesEntityV1
{
    public required long SellerId { get; init; }

    public required long ItemId { get; init; }

    public required string Currency { get; init; }

    public required decimal Price { get; init; }
}