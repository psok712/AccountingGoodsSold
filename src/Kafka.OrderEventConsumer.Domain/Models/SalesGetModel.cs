namespace Kafka.OrderEventConsumer.Domain.Models;

public record SalesGetModel
{
    public required long SellerId { get; init; }

    public required long ItemId { get; init; }
}