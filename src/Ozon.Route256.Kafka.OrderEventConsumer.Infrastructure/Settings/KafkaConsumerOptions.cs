namespace Ozon.Route256.Kafka.OrderEventConsumer.Infrastructure.Settings;

public record KafkaConsumerOptions
{
    public required int ChannelCapacity { get; init; }

    public required long BufferDelaySecond { get; init; }
    
    public required int RetryCount { get; init; }
}