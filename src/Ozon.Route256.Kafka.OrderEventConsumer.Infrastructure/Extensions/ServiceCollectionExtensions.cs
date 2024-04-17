using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Ozon.Route256.Kafka.OrderEventConsumer.Domain;
using Ozon.Route256.Kafka.OrderEventConsumer.Domain.Entities;
using Ozon.Route256.Kafka.OrderEventConsumer.Infrastructure.Repositories;
using Ozon.Route256.Kafka.OrderEventConsumer.Infrastructure.Settings;

namespace Ozon.Route256.Kafka.OrderEventConsumer.Infrastructure.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddInfrastructureRepositories(
        this IServiceCollection services)
    {
        AddPostgresRepositories(services);
        
        return services;
    }
    
    private static void AddPostgresRepositories(IServiceCollection services)
    {
        services.AddSingleton<IItemRepository, ItemRepository>();
    }
    
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services, 
        IConfiguration config)
    {
        Dapper.DefaultTypeMap.MatchNamesWithUnderscores = true;
        
        services.Configure<KafkaOptions>(config.GetSection(nameof(KafkaOptions)));
        services.Configure<KafkaConsumerOptions>(config.GetSection(nameof(KafkaConsumerOptions)));

        services.AddNpgsqlDataSource(
            config.GetSection("ConnectionPostgresString").Value,
            builder =>
            {
                builder.MapComposite<ItemEntityV1>("item_v1", builder.DefaultNameTranslator);
            });
        
        return services;
    }
}