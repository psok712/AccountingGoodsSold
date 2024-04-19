using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using Confluent.Kafka;

namespace Ozon.Route256.Kafka.OrderEventConsumer.Infrastructure.Kafka;

public sealed class SystemTextJsonSerializer<T> : IDeserializer<T>, ISerializer<T>
{
    private readonly JsonSerializerOptions? _jsonSerializerOptions;

    public SystemTextJsonSerializer(JsonSerializerOptions? jsonSerializerOptions = null)
    {
        if (jsonSerializerOptions is null)
            jsonSerializerOptions = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower,
                Converters = { new JsonStringEnumConverter() }
            };

        _jsonSerializerOptions = jsonSerializerOptions;
    }

    public T Deserialize(ReadOnlySpan<byte> data, bool isNull, SerializationContext context)
    {
        return isNull
            ? throw new ArgumentNullException($"Null data encountered deserializing {typeof(T).Name} value.")
            : JsonSerializer.Deserialize<T>(data, _jsonSerializerOptions)!;
    }

    public byte[] Serialize(T data, SerializationContext context)
    {
        return JsonSerializer.SerializeToUtf8Bytes(data, _jsonSerializerOptions);
    }
}