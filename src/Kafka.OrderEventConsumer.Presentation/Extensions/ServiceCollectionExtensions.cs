using Confluent.Kafka;
using Microsoft.Extensions.DependencyInjection;
using Kafka.OrderEventConsumer.Infrastructure.Kafka;
using Kafka.OrderEventConsumer.Infrastructure.OrderProduce;

namespace Kafka.OrderEventConsumer.Presentation.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddKafkaItemHandler(
        this IServiceCollection services)
    {
        services.AddSingleton<IHandler<long, OrderEventProduce>, ItemHandler>();
        services.AddSingleton<IDeserializer<OrderEventProduce>, SystemTextJsonSerializer<OrderEventProduce>>();

        return services;
    }
}