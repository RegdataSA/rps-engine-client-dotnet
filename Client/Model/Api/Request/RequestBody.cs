using System.Collections.Generic;

namespace Regdata.RPS.Engine.Client.Model.Api.Request
{
    public class RequestBody
    {
        public Context.Context LoggingContext { get; set; }
        
        public List<Context.Context> RightsContexts { get; set; } = new List<Context.Context>();

        public List<Context.Context> ProcessingContexts { get; set; } = new List<Context.Context>();
        
        public List<Request> Requests { get; set; } = new List<Request>();
    }
}
