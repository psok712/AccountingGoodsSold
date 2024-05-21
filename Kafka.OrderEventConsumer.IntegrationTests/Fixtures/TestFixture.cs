using FluentMigrator.Runner;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using Moq;
using Kafka.OrderEventConsumer.Domain.Interfaces;
using Kafka.OrderEventConsumer.Infrastructure.Common;
using Kafka.OrderEventConsumer.Infrastructure.Extensions;
using Utils.Extensions;
using Utils.Providers.Interfaces;

namespace Kafka.OrderEventConsumer.IntegrationTests.Fixtures;

public class TestFixture
{
    public readonly Mock<IDateTimeOffsetProvider> DateTimeOffsetProviderFaker = new();

    public TestFixture()
    {
        var config = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json")
            .Build();

        var host = Host.CreateDefaultBuilder()
            .ConfigureServices(services =>
            {
                services
                    .AddInfrastructureRepositories()
                    .AddInfrastructure(config)
                    .AddUtils();

                var connectionString = config["ConnectionPostgresString"]!;
                services
                    .AddFluentMigrator(
                        connectionString,
                        typeof(SqlMigration).Assembly);

                services.Replace(
                    new ServiceDescriptor(typeof(IDateTimeOffsetProvider), DateTimeOffsetProviderFaker.Object));
            })
            .Build();

        ClearDatabase(host);
        using var scope = host.Services.CreateScope();
        var runner = scope.ServiceProvider.GetRequiredService<IMigrationRunner>();

        runner.MigrateUp();

        var serviceProvider = scope.ServiceProvider;
        ItemRepository = serviceProvider.GetRequiredService<IItemRepository>();
        SalesRepository = serviceProvider.GetRequiredService<ISalesRepository>();
    }

    public IItemRepository ItemRepository { get; }

    public ISalesRepository SalesRepository { get; }

    private static void ClearDatabase(IHost host)
    {
        using var scope = host.Services.CreateScope();
        var runner = scope.ServiceProvider.GetRequiredService<IMigrationRunner>();
        runner.MigrateDown(0);
    }
}