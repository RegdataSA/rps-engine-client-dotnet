using System;
using System.Collections.Generic;

namespace Regdata.RPS.Engine.Client.Model.Api.Request
{
    public class Request
    {
        public Guid? Guid { get; set; }
        
        public Context.Context LoggingContext { get; set; }
        
        public Guid RightsContext { get; set; }

        public Guid? ProcessingContext { get; set; }

        public List<Instance> Instances { get; set; } = new List<Instance>();
    }
}
