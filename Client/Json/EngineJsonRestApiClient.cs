using System;
using System.Net;
using System.Threading.Tasks;
using Flurl;
using Flurl.Http;
using Regdata.RPS.Engine.Client.Auth;
using Regdata.RPS.Engine.Client.Extensions;
using Regdata.RPS.Engine.Client.Model.Api.Request;
using Regdata.RPS.Engine.Client.Model.Api.Response;

namespace Regdata.RPS.Engine.Client.Json
{
    public class EngineJsonRestApiClient : IRPSEngineProvider
    {
        public EngineJsonRestApiClient(
            EngineClientOptions clientOptions,
            ITokenProvider tokenProvider)
        {
            HostName = clientOptions.EngineHostName;
            Timeout = clientOptions.Timeout;
            TokenProvider = tokenProvider;
        }

        public TimeSpan? Timeout { get; }

        public string HostName { get; }
        
        public Url Url => HostName.AppendPathSegment(ControllerPath);

        protected ITokenProvider TokenProvider { get; }

        protected string ControllerPath => "api";

        public async Task<ResponseBody> TransformAsync(RequestBody requestBody)
            => await Url.SendAsync(
                r => r.AppendPathSegment("transform")
                    .ConfigureRequest(settings => settings.Timeout = Timeout ?? settings.Timeout)
                    .AllowHttpStatus(HttpStatusCode.BadRequest)
                    .PostJsonAsync(requestBody)
                    .ReceiveJson<ResponseBody>(), TokenProvider);
    }
}
