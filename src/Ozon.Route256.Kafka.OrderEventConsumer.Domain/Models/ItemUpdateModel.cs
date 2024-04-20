namespace Ozon.Route256.Kafka.OrderEventConsumer.Domain.Models;

public record ItemUpdateModel
{
    public required long ItemId { get; init; }

    public required long Quantity { get; init; }
}