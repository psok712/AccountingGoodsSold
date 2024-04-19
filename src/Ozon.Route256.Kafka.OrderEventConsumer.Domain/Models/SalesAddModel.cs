namespace Ozon.Route256.Kafka.OrderEventConsumer.Domain.Models;

public record SalesAddModel
{
    public required long SellerId { get; init; }

    public required long ItemId { get; init; }

    public required string Currency { get; init; }
}