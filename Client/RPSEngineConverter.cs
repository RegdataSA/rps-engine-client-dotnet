using System;
using System.Collections.Generic;
using System.Linq;
using Regdata.RPS.Engine.Client.Extensions;
using Regdata.RPS.Engine.Client.Model.Api.Request;
using Regdata.RPS.Engine.Client.Model.Api.Response;
using Regdata.RPS.Engine.Client.Value;
using ValueError = Regdata.RPS.Engine.Client.Value.ValueError;

namespace Regdata.RPS.Engine.Client
{
    public class RPSEngineConverter
    {
        public RequestBody ToRequestBody(RequestContext requestContext)
        {
            var requestBody = new RequestBody();

            if (!requestContext.Requests.Any())
                return requestBody;

            var rightsContextsByGuid = new Dictionary<Model.Api.Context.Context, Guid>();
            var processingContextsByGuid = new Dictionary<Model.Api.Context.Context, Guid>();

            foreach (Request request in requestContext.Requests)
            {
                if (!request.Values.Any())
                    continue;

                Model.Api.Context.Context rightsContext = ToModel<Model.Api.Context.Context>(request.RightsContext);
                if (!rightsContextsByGuid.TryGetValue(rightsContext, out Guid rightsContextGuid))
                {
                    rightsContextGuid = Guid.NewGuid();
                    rightsContext.Guid = rightsContextGuid;

                    rightsContextsByGuid[rightsContext] = rightsContextGuid;

                    requestBody.RightsContexts.Add(rightsContext);
                }

                Model.Api.Context.Context processingContext = ToModel<Model.Api.Context.Context>(request.ProcessingContext);
                Model.Api.Context.Context loggingContext = ToModel<Model.Api.Context.Context>(request.LoggingContext);

                if (processingContext == null)
                {
                    requestBody.Requests.Add(ToRequest(request.Values,
                        request.Guid,
                        rightsContextGuid,
                        null,
                        loggingContext: loggingContext));
                    continue;
                }

                if (!processingContextsByGuid.TryGetValue(processingContext, out Guid processingContextGuid))
                {
                    processingContextGuid = Guid.NewGuid();
                    processingContext.Guid = processingContextGuid;

                    processingContextsByGuid[processingContext] = processingContextGuid;

                    requestBody.ProcessingContexts.Add(processingContext);
                }

                requestBody.Requests.Add(ToRequest(request.Values,
                    request.Guid,
                    rightsContextGuid,
                    processingContextGuid,
                    loggingContext: loggingContext));
            }

            return requestBody;
        }

        public void FromResponseBody(ResponseBody responseBody, RequestContext requestContext)
        {
            if (responseBody.Error != null)
                throw new RPSEngineException("Error received from RPS Engine API response. " +
                                             $"Code: '{responseBody.Error.Code}'. " +
                                             $"Message: '{responseBody.Error.Message}'.", responseBody.Error);

            foreach (Response response in responseBody.Responses)
            {
                if (!requestContext.TryGetRequest(response.Request, out Request request))
                    continue;

                for (int i = 0; i < response.Instances.Count; i++)
                {
                    Model.Api.Instance responseInstance = response.Instances[i];
                    request.Values[i].Value = responseInstance.Value;

                    if (responseInstance.Error != null)
                        request.Values[i].Error = new ValueError(
                            responseInstance.Error.Code,
                            responseInstance.Error.Message);
                }
            }
        }

        public void AssignNullValues(RequestContext requestContext)
        {
            foreach (Request request in requestContext.Requests)
            {
                foreach (IRPSValue rpsValue in request.Values)
                {
                    rpsValue.Value = null;
                }
            }
        }

        private static Model.Api.Request.Request ToRequest(
            IEnumerable<IRPSValue> rpsValues,
            Guid requestGuid,
            Guid rightsContextGuid,
            Guid? processingContextGuid,
            Model.Api.Context.Context loggingContext = null)
        {
            var request = new Model.Api.Request.Request
            {
                Guid = requestGuid,
                RightsContext = rightsContextGuid,
                ProcessingContext = processingContextGuid,
                LoggingContext = loggingContext
            };

            foreach (var rpsValue in rpsValues)
            {
                var instance = new Model.Api.Instance {Value = rpsValue.Value};
                request.Instances.Add(instance);

                if (rpsValue.Instance != null)
                {
                    instance.ClassName = rpsValue.Instance.ClassName;
                    instance.PropertyName = rpsValue.Instance.PropertyName;
                }

                if (rpsValue.Dependencies.Any())
                    instance.DependencyContext = rpsValue.Dependencies.ToContext();
            }

            return request;
        }

        private static Model.Api.Context.Context ToModel<T>(Context context, Func<T, T> modifyContext = null)
            where T : Model.Api.Context.Context, new()
        {
            if (context == null)
                return default;

            var modelContext = new T
            {
                Evidences = new List<Model.Api.Context.Evidence>(
                    context.Evidences.Select(ToModel))
            };

            return modifyContext?.Invoke(modelContext) ?? modelContext;
        }

        private static Model.Api.Context.Evidence ToModel(Evidence evidence)
            => new Model.Api.Context.Evidence
            {
                Name = evidence.Name,
                Value = evidence.Value
            };
    }
}
