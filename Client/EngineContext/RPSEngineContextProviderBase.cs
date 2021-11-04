using System;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace Regdata.RPS.Engine.Client.EngineContext
{
    public abstract class RPSEngineContextProviderBase : IRPSEngineContextProvider
    {
        private ConcurrentDictionary<string, RightsContext> _rightsContextsByKey;
        private ConcurrentDictionary<string, ProcessingContext> _processingContextsByKey;
        
        public void Initialize()
        {
            _rightsContextsByKey = new ConcurrentDictionary<string, RightsContext>(GetRightsContexts());
            _processingContextsByKey = new ConcurrentDictionary<string, ProcessingContext>(GetProcessingContexts());
        }

        public bool TryGetRightsContext(string contextKey, out RightsContext rightsContext)
        {
            if (_rightsContextsByKey == null)
                throw new InvalidOperationException("Context provider is not initialized");

            return _rightsContextsByKey.TryGetValue(contextKey, out rightsContext);
        }

        public bool TryGetProcessingContext(string contextKey, out ProcessingContext processingContext)
        {
            if (_processingContextsByKey == null)
                throw new InvalidOperationException("Context provider is not initialized");

            return _processingContextsByKey.TryGetValue(contextKey, out processingContext);
        }

        protected abstract IReadOnlyDictionary<string, RightsContext> GetRightsContexts();

        protected abstract IReadOnlyDictionary<string, ProcessingContext> GetProcessingContexts();
    }
}
