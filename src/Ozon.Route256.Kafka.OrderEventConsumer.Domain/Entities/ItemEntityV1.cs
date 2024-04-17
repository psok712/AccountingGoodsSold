using System;

namespace Ozon.Route256.Kafka.OrderEventConsumer.Domain.Entities;

public record ItemEntityV1
{
    public required long ItemId { get; init; }
    
    public required long Created { get; init; }
    
    public required long Delivered { get; init; }
    
    public required long Cancelled { get; init; }
    
    public required DateTimeOffset CreatedAt { get; init; }
}