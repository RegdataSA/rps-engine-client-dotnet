using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Regdata.RPS.Engine.Client.EngineContext;
using Regdata.RPS.Engine.Client.Value;

namespace Regdata.RPS.Engine.Client
{
    public class RequestContext
    {
        private readonly ConcurrentDictionary<Guid, Request> _requestsByGuid
            = new ConcurrentDictionary<Guid, Request>();

        private readonly RPSEngine _engine;
        private readonly RPSEngineContextResolver _engineContextResolver;

        internal RequestContext(RPSEngine engine, RPSEngineContextResolver contextResolver = null)
        {
            _engine = engine;
            _engineContextResolver = contextResolver;
        }

        public IEnumerable<Request> Requests => _requestsByGuid.Values;

        public bool TryGetRequest(Guid requestGuid, out Request request)
            => _requestsByGuid.TryGetValue(requestGuid, out request);

        public void Transform() => _engine.Transform(this);

        public async Task TransformAsync() => await _engine.TransformAsync(this);

        public RequestContext WithRequest(IEnumerable<IRPSValue> rpsValues,
            string rightsContextName,
            string processingContextName,
            Context loggingContext = null)
        {
            if (_engineContextResolver == null)
                throw new InvalidOperationException("Context resolver not found");

            (Context RightsContext, ProcessingContext ProcessingContext) contexts =
                _engineContextResolver.Resolve(rightsContextName, processingContextName);

            return WithRequest(rpsValues, 
                contexts.RightsContext, 
                contexts.ProcessingContext, 
                loggingContext: loggingContext);
        }

        public RequestContext WithRequest(IEnumerable<IRPSValue> rpsValues,
            Context rightsContext,
            ProcessingContext processingContext,
            Context loggingContext = null)
        {
            var request = new Request
            {
                Values = rpsValues.ToArray(),
                RightsContext = rightsContext,
                ProcessingContext = processingContext,
                LoggingContext = loggingContext
            };

            _requestsByGuid[request.Guid] = request;

            return this;
        }
    }
}
