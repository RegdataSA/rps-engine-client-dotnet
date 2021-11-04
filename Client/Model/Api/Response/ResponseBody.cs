using System.Collections.Generic;

namespace Regdata.RPS.Engine.Client.Model.Api.Response
{
    public class ResponseBody
    {
        public List<Response> Responses { get; set; } = new List<Response>();
        
        public Error.Error Error { get; set; }
    }
}
