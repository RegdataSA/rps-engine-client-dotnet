using System.Threading.Tasks;
using Regdata.RPS.Engine.Client.Model.Api.Request;
using Regdata.RPS.Engine.Client.Model.Api.Response;

namespace Regdata.RPS.Engine.Client
{
    public interface IRPSEngineProvider
    {
        Task<ResponseBody> TransformAsync(RequestBody requestBody);
    }
}
