using Confluent.Kafka;
using Microsoft.Extensions.DependencyInjection;
using Ozon.Route256.Kafka.OrderEventConsumer.Domain.OrderProduce;
using Ozon.Route256.Kafka.OrderEventConsumer.Infrastructure.Kafka;

namespace Ozon.Route256.Kafka.OrderEventConsumer.Presentation.Extensions;

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