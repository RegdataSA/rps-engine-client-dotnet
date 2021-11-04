using System.Threading.Tasks;
using Flurl.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Regdata.RPS.Engine.Client.Examples.Examples;
using Regdata.RPS.Engine.Client.Extensions;
using Regdata.RPS.Engine.Client.NetCore.Extensions;

namespace Regdata.RPS.Engine.Client.Examples
{
    class Program
    {
        static async Task Main(string[] args)
        {
            await CreateHostBuilder(args).Build().RunAsync();
        }

        private static IHostBuilder CreateHostBuilder(string[] args)
        {
            return Host
                .CreateDefaultBuilder(args)
                .ConfigureServices(
                    (context, services) =>
                    {
                        services.AddRPSEngine(context.Configuration.GetSection("RPSEngine"));

                        // Uncomment if you want to use rights and processing contexts from appsettings.json.
                        // WARNING! Comment another 'AddRPSEngineContextJsonFileProvider' provider.
                        //services.AddRPSEngineContextConfigurationProvider();

                        // Uncomment if you want to use rihts and processing context from custom files.
                        // WARNING! Comment another 'AddRPSEngineContextConfigurationProvider' provider.
                        services.AddRPSEngineContextJsonFileProvider(@"Data\RightsContexts.json", @"Data\ProcessingContexts.json");

                        FlurlHttp.Configure(s => s.UseDefaultJsonSettings());

                        services
                            .AddHostedService<SimpleUsageExample>()
                            .AddHostedService<ContextsProvidedByResolverExample>()
                            .AddHostedService<UsageWithDependenciesExample>()
                            .AddHostedService<UsageWithRelatedObjectExample>();
                    });
        }
    }
}
