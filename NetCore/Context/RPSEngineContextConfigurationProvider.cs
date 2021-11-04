using System.Collections.Generic;
using Microsoft.Extensions.Configuration;
using Regdata.RPS.Engine.Client.EngineContext;

namespace Regdata.RPS.Engine.Client.NetCore.Context
{
    /// <summary>
    /// Implementation of the IRPSEngineContextProvider interface based on IConfiguration sources.
    /// Used in order to extract rights and processing contexts from sections of configuration at appsettings.json 
    /// </summary>
    public class RPSEngineContextConfigurationProvider : RPSEngineContextProviderBase
    {
        private readonly IConfiguration _rightsContextsConfiguration;
        private readonly IConfiguration _processingContextsConfiguration;

        public RPSEngineContextConfigurationProvider(IConfiguration rightsContextsConfiguration,
            IConfiguration processingContextsConfiguration)
        {
            _rightsContextsConfiguration = rightsContextsConfiguration;
            _processingContextsConfiguration = processingContextsConfiguration;
        }

        protected override IReadOnlyDictionary<string, RightsContext> GetRightsContexts()
            => _rightsContextsConfiguration.Get<Dictionary<string, RightsContext>>();

        protected override IReadOnlyDictionary<string, ProcessingContext> GetProcessingContexts()
            => _processingContextsConfiguration.Get<Dictionary<string, ProcessingContext>>();
    }
}
