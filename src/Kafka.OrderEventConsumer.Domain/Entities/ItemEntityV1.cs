using System;

namespace Kafka.OrderEventConsumer.Domain.Entities;

public record ItemEntityV1
{
    public required long ItemId { get; init; }

    public required long Created { get; init; }

    public required long Delivered { get; init; }

    public required long Canceled { get; init; }

    public required DateTimeOffset UpdatedAt { get; init; }
}