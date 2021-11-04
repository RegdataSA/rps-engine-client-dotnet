using System;
using System.Collections.Generic;

namespace Regdata.RPS.Engine.Client.Model.Api.Response
{
    public class Response
    {
        public Guid Request { get; set; }

        public Guid? RightsContext { get; set; }

        public Guid? ProcessingContext { get; set; }

        public List<Instance> Instances { get; set; } = new List<Instance>();
    }
}
