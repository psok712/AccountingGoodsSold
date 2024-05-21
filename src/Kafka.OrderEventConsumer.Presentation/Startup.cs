using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Kafka.OrderEventConsumer.Infrastructure.Common;
using Kafka.OrderEventConsumer.Infrastructure.Extensions;
using Kafka.OrderEventConsumer.Presentation.Extensions;
using Utils.Extensions;

namespace Kafka.OrderEventConsumer.Presentation;

public sealed class Startup
{
    private readonly IConfiguration _configuration;

    public Startup(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public void ConfigureServices(IServiceCollection services)
    {
        services
            .AddKafkaService()
            .AddKafkaItemHandler()
            .AddUtils()
            .AddInfrastructureRepositories()
            .AddInfrastructure(_configuration)
            .AddLogging();

        var connectionString = _configuration["ConnectionPostgresString"]!;

        services
            .AddFluentMigrator(
                connectionString,
                typeof(SqlMigration).Assembly);

        services.AddHostedService<KafkaBackgroundService>();
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
    }
}