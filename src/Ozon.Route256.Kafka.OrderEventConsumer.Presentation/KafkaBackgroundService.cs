using System;
using System.Threading;
using System.Threading.Tasks;
using Confluent.Kafka;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Ozon.Route256.Kafka.OrderEventConsumer.Infrastructure.Kafka;
using Ozon.Route256.Kafka.OrderEventConsumer.Infrastructure.Settings;

namespace Ozon.Route256.Kafka.OrderEventConsumer.Presentation;

public class KafkaBackgroundService : BackgroundService
{
    private readonly KafkaAsyncConsumer<Ignore, string> _consumer;
    private readonly ILogger<KafkaBackgroundService> _logger;

    public KafkaBackgroundService(
        IServiceProvider serviceProvider,
        ILogger<KafkaBackgroundService> logger,
        IOptions<KafkaOptions> kafkaOptions,
        IOptions<KafkaConsumerOptions> kafkaConsumerOptions)
    {
        var kafkaOptionsValue = kafkaOptions.Value;
        _logger = logger;
        var handler = serviceProvider.GetRequiredService<ItemHandler>();
        _consumer = new KafkaAsyncConsumer<Ignore, string>(
            handler,
            kafkaOptionsValue.BootstrapServers,
            kafkaOptionsValue.GroupId,
            kafkaOptionsValue.Topic,
            null,
            null,
            serviceProvider.GetRequiredService<ILogger<KafkaAsyncConsumer<Ignore, string>>>(),
            kafkaConsumerOptions);
    }

    public override Task StopAsync(CancellationToken cancellationToken)
    {
        _consumer.Dispose();

        return Task.CompletedTask;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        try
        {
            await _consumer.Consume(stoppingToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unhandled exception occured");
        }
    }
}