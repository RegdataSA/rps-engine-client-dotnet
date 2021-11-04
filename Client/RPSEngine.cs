using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Regdata.RPS.Engine.Client.EngineContext;
using Regdata.RPS.Engine.Client.Extensions;
using Regdata.RPS.Engine.Client.Model.Api.Request;
using Regdata.RPS.Engine.Client.Model.Api.Response;
using Regdata.RPS.Engine.Client.Value;

namespace Regdata.RPS.Engine.Client
{
    public class RPSEngine
    {
        private readonly IRPSEngineProvider _provider;
        private readonly RPSEngineConverter _converter;
        private readonly RPSEngineContextResolver _engineContextResolver;

        public RPSEngine(IRPSEngineProvider engineProvider,
            RPSEngineConverter converter,
            RPSEngineContextResolver contextResolver = null)
        {
            _provider = engineProvider;
            _converter = converter;
            _engineContextResolver = contextResolver;
        }

        public RequestContext CreateContext() => new RequestContext(this, _engineContextResolver);

        public void Transform(IEnumerable<IRPSValue> rpsValues,
            string rightsContextName,
            string processingContextName,
            Context loggingContext = null) =>
            TransformAsync(rpsValues,
                rightsContextName,
                processingContextName,
                loggingContext: loggingContext).GetAwaitedResult();

        public void Transform(IEnumerable<IRPSValue> rpsValues,
            Context rightsContext,
            ProcessingContext processingContext,
            Context loggingContext = null) =>
            TransformAsync(rpsValues,
                rightsContext,
                processingContext,
                loggingContext: loggingContext).GetAwaitedResult();

        public void Transform(RequestContext requestContext) => TransformAsync(requestContext).GetAwaitedResult();

        public async Task TransformAsync(IEnumerable<IRPSValue> rpsValues,
            string rightsContextName,
            string processingContextName,
            Context loggingContext = null)
        {
            if (_engineContextResolver == null)
                throw new InvalidOperationException("Context resolver not found");

            (Context RightsContext, ProcessingContext ProcessingContext) contexts =
                _engineContextResolver.Resolve(rightsContextName, processingContextName);

            await TransformAsync(rpsValues, 
                contexts.RightsContext, 
                contexts.ProcessingContext, 
                loggingContext: loggingContext);
        }

        public async Task TransformAsync(IEnumerable<IRPSValue> rpsValues,
            Context rightsContext,
            ProcessingContext processingContext,
            Context loggingContext = null)
            => await TransformAsync(CreateContext().WithRequest(rpsValues,
                rightsContext,
                processingContext,
                loggingContext: loggingContext));

        public virtual async Task TransformAsync(RequestContext requestContext)
        {
            RequestBody requestBody = _converter.ToRequestBody(requestContext);

            try
            {
                ResponseBody responseBody = await _provider.TransformAsync(requestBody);
                _converter.FromResponseBody(responseBody, requestContext);
            }
            catch
            {
                _converter.AssignNullValues(requestContext);
                throw;
            }
        }
    }
}
