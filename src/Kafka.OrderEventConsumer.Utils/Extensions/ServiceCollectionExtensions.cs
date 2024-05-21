using Microsoft.Extensions.DependencyInjection;
using Utils.Providers;
using Utils.Providers.Interfaces;

namespace Utils.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddUtils(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddSingleton<IDateTimeOffsetProvider, DateTimeOffsetProvider>();

        return serviceCollection;
    }
}