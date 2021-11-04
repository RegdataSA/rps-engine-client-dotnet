using System;
using Regdata.RPS.Engine.Client.EngineContext;
using Regdata.RPS.Engine.Client.Value;

namespace Regdata.RPS.Engine.Client
{
    public class Request
    {
        public Guid Guid { get; } = Guid.NewGuid();

        public IRPSValue[] Values { get; set; }

        public Context RightsContext { get; set; }

        public ProcessingContext ProcessingContext { get; set; }

        public Context LoggingContext { get; set; }
    }
}
