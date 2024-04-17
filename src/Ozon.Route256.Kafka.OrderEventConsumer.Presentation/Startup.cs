using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Ozon.Route256.Kafka.OrderEventConsumer.Infrastructure.Common;
using Ozon.Route256.Kafka.OrderEventConsumer.Infrastructure.Extensions;
using Utils.Extensions;

namespace Ozon.Route256.Kafka.OrderEventConsumer.Presentation;

public sealed class Startup
{
    private readonly IConfiguration _configuration;

    public Startup(IConfiguration configuration) => _configuration = configuration;

    public void ConfigureServices(IServiceCollection services)
    {
        services
            .AddUtils()
            .AddInfrastructureRepositories()
            .AddInfrastructure(_configuration)
            .AddLogging();

        var connectionString = _configuration["ConnectionPostgresString"]!;
        
        services
            .AddFluentMigrator(
                connectionString,
                typeof(SqlMigration).Assembly);
        
        services.AddSingleton<ItemHandler>();
        services.AddHostedService<KafkaBackgroundService>();
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
    }
}
