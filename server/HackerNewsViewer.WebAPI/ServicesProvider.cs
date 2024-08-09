using HackerNewsViewer.WebAPI.HttpClients;
using HackerNewsViewer.WebAPI.Services;
using Microsoft.Extensions.Caching.Memory;

namespace HackerNewsViewer
{
    // This is a great solution when you need to set up dependency injection
    // from a project outside of this one such as an integration test project.
    // Giving credit where credit is due.
    // Sourced from: https://gkama.medium.com/dependency-injection-di-in-net-core-and-net-5-c-unit-tests-935651a99a2d

    public static class ServicesProvider
    {
        public static IServiceProvider WireUpDependencyInjection(IServiceCollection? services = null)
        {
            services ??= new ServiceCollection();

            services.AddHttpClient(Constants.HackerNewsHttpClient);

            services.AddMemoryCache(opts =>
            {
                opts.ExpirationScanFrequency = TimeSpan.FromMinutes(55);
            });

            services.AddScoped<IHackerNewsHttpClient, HackerNewsHttpClient>();
            services.AddSingleton<ICacheService, CacheService>(provider =>
            {
                return new CacheService(provider.GetService<IMemoryCache>(), 60, 20);
            });

            return services.BuildServiceProvider();
        }

        public static T GetRequiredService<T>() where T : notnull
        {
            var provider = ServicesProvider.WireUpDependencyInjection();

            return provider.GetRequiredService<T>();
        }
    }
}
