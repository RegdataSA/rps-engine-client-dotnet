using System.Net.Http;
using System.Threading.Tasks;
using Flurl;
using IdentityModel;
using IdentityModel.Client;

namespace Regdata.RPS.Engine.Client.Auth
{
    public class ClientCredentialsTokenProvider : ITokenProvider
    {
        private readonly EngineClientOptions _clientOptions;
        private TokenResponse _current;

        public ClientCredentialsTokenProvider(EngineClientOptions clientOptions)
        {
            _clientOptions = clientOptions;
        }

        public async Task<TokenResponse> GetCurrentAsync() 
            => _current ?? (_current = await RequestNewAsync());

        public async Task<TokenResponse> RequestNewAsync()
        {
            using (var httpClient = new HttpClient())
            {
                return _current = await httpClient.RequestTokenAsync(await GetTokenRequestAsync());
            }
        }

        protected Task<TokenRequest> GetTokenRequestAsync()
            => Task.FromResult(GetTokenRequest());
        
        private TokenRequest GetTokenRequest()
            => new ClientCredentialsTokenRequest
            {
                Address = _clientOptions.IdentityServerHostName.AppendPathSegment("connect/token"),
                GrantType = OidcConstants.GrantTypes.ClientCredentials,
                ClientId = _clientOptions.ApiKey,
                ClientSecret = _clientOptions.SecretKey
            };
    }
}
