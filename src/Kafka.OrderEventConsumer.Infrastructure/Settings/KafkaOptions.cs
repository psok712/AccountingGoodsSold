namespace Kafka.OrderEventConsumer.Infrastructure.Settings;

public record KafkaOptions
{
    public required string BootstrapServers { get; init; } = string.Empty;

    public required string GroupId { get; init; } = string.Empty;

    public required string Topic { get; init; } = string.Empty;
}