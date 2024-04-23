using Dapper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Ozon.Route256.Kafka.OrderEventConsumer.Domain.Interfaces;
using Ozon.Route256.Kafka.OrderEventConsumer.Infrastructure.Repositories;
using Ozon.Route256.Kafka.OrderEventConsumer.Infrastructure.Services;
using Ozon.Route256.Kafka.OrderEventConsumer.Infrastructure.Settings;

namespace Ozon.Route256.Kafka.OrderEventConsumer.Infrastructure.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddInfrastructureRepositories(this IServiceCollection services)
    {
        AddPostgresRepositories(services);

        return services;
    }

    public static IServiceCollection AddKafkaService(this IServiceCollection services)
    {
        services.AddSingleton<IKafkaService, KafkaService>();

        return services;
    }

    private static void AddPostgresRepositories(IServiceCollection services)
    {
        services.AddSingleton<IItemRepository, ItemRepository>();
        services.AddSingleton<ISalesRepository, SalesRepository>();
    }

    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration config)
    {
        DefaultTypeMap.MatchNamesWithUnderscores = true;

        services.Configure<KafkaOptions>(config.GetSection(nameof(KafkaOptions)));
        services.Configure<KafkaConsumerOptions>(config.GetSection(nameof(KafkaConsumerOptions)));

        services.AddNpgsqlDataSource(config.GetSection("ConnectionPostgresString").Value);

        return services;
    }
}