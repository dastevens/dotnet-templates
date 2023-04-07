namespace dastevens.Console
{
    using System.Reflection;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;

    internal static class Program
    {
        internal static async Task Main(string[] args)
        {
            var builder = new HostBuilder()
                .ConfigureAppConfiguration((hostingContext, config) =>
                {
                    config.AddJsonFile("appsettings.json", optional: true);
                    config.AddEnvironmentVariables();
                    config.AddCommandLine(args);
                })
                .ConfigureServices((hostingContext, serviceCollection) =>
                {
                    serviceCollection.AddOptions();
                    serviceCollection.AddSingleton<Cli>();
                    serviceCollection.AddVerbs();
                });

            var host = builder.Build();
            var cli = host.Services.GetService<Cli>();

            await cli.RunAsync(args, default);
        }

        private static IServiceCollection AddVerbs(this IServiceCollection serviceCollection)
        {
            var verbTypes = AppDomain.CurrentDomain
                .GetAssemblies()
                .SelectMany(s => s.GetTypes())
                .Where(x => !x.IsAbstract && x.IsClass && typeof(IVerb).IsAssignableFrom(x));

            foreach (var verbType in verbTypes)
            {
                serviceCollection.AddSingleton(verbType);
            }

            return serviceCollection;
        }
    }
}