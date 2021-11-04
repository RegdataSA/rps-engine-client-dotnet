using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Regdata.RPS.Engine.Client.Auth;
using Regdata.RPS.Engine.Client.EngineContext;
using Regdata.RPS.Engine.Client.Json;
using Regdata.RPS.Engine.Client.NetCore.Context;

namespace Regdata.RPS.Engine.Client.NetCore.Extensions
{
    public static class RPSEngineExtensions
    {
        /// <summary>
        /// Adding all dependencies of RPSEngine class to DI in order to start using it.
        /// </summary>
        /// <param name="services">Services instance</param>
        /// <param name="engineSection">Reference to RPSEngine config section. Example: Configuration.GetSection("RPSEngine")</param>
        /// <returns>Services instance</returns>
        public static IServiceCollection AddRPSEngine(this IServiceCollection services, 
            IConfigurationSection engineSection)
            => services
                .Configure<EngineClientOptions>(engineSection)
                .AddSingleton(ctx => ctx.GetRequiredService<IOptions<EngineClientOptions>>().Value)
                .AddSingleton<ITokenProvider, ClientCredentialsTokenProvider>()
                .AddTransient<IRPSEngineProvider, EngineJsonRestApiClient>()
                .AddTransient<RPSEngineConverter>()
                .AddTransient<RPSEngine>();

        /// <summary>
        /// Adding RPSEngine JSON files source of rights and processing context.
        /// </summary>
        /// <param name="services">Services instance</param>
        /// <param name="rightsContextFilePath">Path to JSON file of rights context</param>
        /// <param name="processingContextFilePath">Path to JSON file of processing context</param>
        /// <returns>Services instance</returns>
        public static IServiceCollection AddRPSEngineContextJsonFileProvider(this IServiceCollection services, 
            string rightsContextFilePath, 
            string processingContextFilePath)
            => services
                .AddTransient<RPSEngineContextResolver>()
                .AddSingleton<IRPSEngineContextProvider>(provider =>
                {
                    var rpsEngineContextProvider = new RPSEngineContextJsonFileProvider(
                        rightsContextFilePath,
                        processingContextFilePath);

                    rpsEngineContextProvider.Initialize();
                    return rpsEngineContextProvider;
                });

        /// <summary>
        /// Adding RPSEngine configuration file source of rights and processing context.
        /// Require to specify "RightsContexts" and "ProcessingContexts" sections at the root of configuration file (appsettings.json).
        /// </summary>
        /// <param name="services">Services instance</param>
        /// <returns>Services instance</returns>
        public static IServiceCollection AddRPSEngineContextConfigurationProvider(this IServiceCollection services)
            => services
                .AddTransient<RPSEngineContextResolver>()
                .AddSingleton<IRPSEngineContextProvider>(provider =>
                {
                    var configuration = provider.GetRequiredService<IConfiguration>();

                    var rpsEngineContextProvider = new RPSEngineContextConfigurationProvider(
                        configuration.GetSection("RightsContexts"),
                        configuration.GetSection("ProcessingContexts"));

                    rpsEngineContextProvider.Initialize();
                    return rpsEngineContextProvider;
                });
    }
}
