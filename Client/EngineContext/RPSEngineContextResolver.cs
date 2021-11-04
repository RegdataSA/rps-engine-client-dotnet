using System;

namespace Regdata.RPS.Engine.Client.EngineContext
{
    public class RPSEngineContextResolver
    {
        private readonly IRPSEngineContextProvider _contextProvider;

        public RPSEngineContextResolver(IRPSEngineContextProvider contextProvider)
        {
            _contextProvider = contextProvider;
        }

        public (RightsContext RightsContext, ProcessingContext ProcessingContext) Resolve(string rightsContextKey, string processingContextKey)
        {
            if (!_contextProvider.TryGetRightsContext(rightsContextKey, out RightsContext rightsContext))
                throw new Exception($"Rights context = '{rightsContextKey}' not found");

            if (!_contextProvider.TryGetProcessingContext(processingContextKey, out ProcessingContext processingContext))
                throw new Exception($"Processing context = '{processingContextKey}' not found");

            return (rightsContext, processingContext);
        }
    }
}
