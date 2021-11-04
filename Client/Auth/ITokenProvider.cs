using System.Threading.Tasks;
using IdentityModel.Client;

namespace Regdata.RPS.Engine.Client.Auth
{
    public interface ITokenProvider
    {
        Task<TokenResponse> GetCurrentAsync();

        Task<TokenResponse> RequestNewAsync();
    }
}
