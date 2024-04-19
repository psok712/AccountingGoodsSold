using System;
using System.Linq;
using System.Threading;
using System.Threading.Channels;
using System.Threading.Tasks;
using Confluent.Kafka;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Ozon.Route256.Kafka.OrderEventConsumer.Domain.Common;
using Ozon.Route256.Kafka.OrderEventConsumer.Infrastructure.Settings;
using Polly;

namespace Ozon.Route256.Kafka.OrderEventConsumer.Infrastructure.Kafka;

public sealed class KafkaAsyncConsumer<TKey, TValue> : IDisposable
{
    private readonly TimeSpan _bufferDelay;

    private readonly Channel<ConsumeResult<TKey, TValue>> _channel;
    private readonly int _channelCapacity;
    private readonly IConsumer<TKey, TValue> _consumer;
    private readonly IHandler<TKey, TValue> _handler;

    private readonly ILogger<KafkaAsyncConsumer<TKey, TValue>> _logger;

    public KafkaAsyncConsumer(
        IHandler<TKey, TValue> handler,
        string bootstrapServers,
        string groupId,
        string topic,
        IDeserializer<TKey>? keyDeserializer,
        IDeserializer<TValue>? valueDeserializer,
        ILogger<KafkaAsyncConsumer<TKey, TValue>> logger,
        IOptions<KafkaConsumerOptions> kafkaConsumerOptions)
    {
        _channelCapacity = kafkaConsumerOptions.Value.ChannelCapacity;
        _bufferDelay = TimeSpan.FromSeconds(kafkaConsumerOptions.Value.BufferDelaySecond);

        var builder = new ConsumerBuilder<TKey, TValue>(
            new ConsumerConfig
            {
                BootstrapServers = bootstrapServers,
                GroupId = groupId,
                AutoOffsetReset = AutoOffsetReset.Earliest,
                EnableAutoCommit = true,
                EnableAutoOffsetStore = false
            });

        if (keyDeserializer is not null) builder.SetKeyDeserializer(keyDeserializer);

        if (valueDeserializer is not null) builder.SetValueDeserializer(valueDeserializer);

        _handler = handler;
        _logger = logger;

        _channel = Channel.CreateBounded<ConsumeResult<TKey, TValue>>(
            new BoundedChannelOptions(_channelCapacity)
            {
                SingleWriter = true,
                SingleReader = true,
                AllowSynchronousContinuations = true,
                FullMode = BoundedChannelFullMode.Wait
            });

        _consumer = builder.Build();
        _consumer.Subscribe(topic);
    }

    public void Dispose()
    {
        _consumer.Close();
    }

    public Task Consume(CancellationToken token)
    {
        var handle = HandleCore(token);
        var consume = ConsumeCore(token);

        return Task.WhenAll(handle, consume);
    }

    private async Task HandleCore(CancellationToken token)
    {
        await Task.Yield();

        await foreach (var consumeResults in _channel.Reader
                           .ReadAllAsync(token)
                           .Buffer(_channelCapacity, _bufferDelay)
                           .WithCancellation(token))
        {
            token.ThrowIfCancellationRequested();

            var policy = Policy
                .Handle<Exception>()
                .RetryAsync(async (exception, retryCount) =>
                {
                    _logger.LogError(exception, $"Unhandled exception occurred. Retry count: {retryCount}");
                    await Task.Delay(1000, token);
                });

            await policy.ExecuteAsync(async () =>
            {
                await _handler.Handle(consumeResults, token);

                var partitionLastOffsets = consumeResults
                    .GroupBy(
                        r => r.Partition.Value,
                        (_, f) => f.MaxBy(p => p.Offset.Value));

                foreach (var partitionLastOffset in partitionLastOffsets)
                    _consumer.StoreOffset(partitionLastOffset);
            });
        }
    }

    private async Task ConsumeCore(CancellationToken token)
    {
        await Task.Yield();

        while (_consumer.Consume(token) is { } result)
        {
            await _channel.Writer.WriteAsync(result, token);
            _logger.LogTrace(
                "{Partition}:{Offset}:WriteToChannel",
                result.Partition.Value,
                result.Offset.Value);
        }

        _channel.Writer.Complete();
    }
}