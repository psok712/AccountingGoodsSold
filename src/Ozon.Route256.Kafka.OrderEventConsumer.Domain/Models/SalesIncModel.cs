namespace Ozon.Route256.Kafka.OrderEventConsumer.Domain.Models;

public record SalesIncModel
{
    public required long SellerId { get; init; }

    public required long ItemId { get; init; }

    public required decimal Price { get; init; }
}