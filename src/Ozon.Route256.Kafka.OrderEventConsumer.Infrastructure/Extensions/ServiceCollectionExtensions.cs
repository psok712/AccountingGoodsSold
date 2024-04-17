using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Ozon.Route256.Kafka.OrderEventConsumer.Infrastructure.Settings;

namespace Ozon.Route256.Kafka.OrderEventConsumer.Infrastructure.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddBllInfrastructure(
        this IServiceCollection services, 
        IConfiguration config)
    {
        //read config
        services.Configure<KafkaOptions>(config.GetSection(nameof(KafkaOptions)));
        services.Configure<KafkaConsumerOptions>(config.GetSection(nameof(KafkaConsumerOptions)));
        
        return services;
    }
}