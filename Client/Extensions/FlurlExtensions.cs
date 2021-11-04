using System;
using System.Net;
using System.Threading.Tasks;
using Flurl;
using Flurl.Http;
using Flurl.Http.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Regdata.RPS.Engine.Client.Auth;

namespace Regdata.RPS.Engine.Client.Extensions
{
    public static class FlurlExtensions
    {
        public static GlobalFlurlHttpSettings UseDefaultJsonSettings(this GlobalFlurlHttpSettings settings)
        {
            var jsonSettings = new JsonSerializerSettings { NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore };
            jsonSettings.Converters.Add(new StringEnumConverter());
            settings.JsonSerializer = new NewtonsoftJsonSerializer(jsonSettings);

            return settings;
        }

        public static async Task<T> SendAsync<T>(this Url url,
            Func<IFlurlRequest, Task<T>> func,
            ITokenProvider tokenProvider = null)
        {
            try
            {
                return await new FlurlRequest(url.Clone()).SendAsync(func, tokenProvider);
            }
            catch (FlurlHttpException e) when (e.StatusCode == (int)HttpStatusCode.Unauthorized)
            {
                if (tokenProvider != null)
                    await tokenProvider.RequestNewAsync();

                return await new FlurlRequest(url.Clone()).SendAsync(func, tokenProvider);
            }
        }

        private static async Task<T> SendAsync<T>(this IFlurlRequest flurlRequest,
            Func<IFlurlRequest, Task<T>> func,
            ITokenProvider tokenProvider = null)
        {
            if (tokenProvider == null)
                return await func(flurlRequest);

            string accessToken = (await tokenProvider.GetCurrentAsync()).AccessToken;
            return await func(flurlRequest.WithOAuthBearerToken(accessToken));
        }
    }
}
