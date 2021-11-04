using System;

namespace Regdata.RPS.Engine.Client.Value
{
    public class ValueError
    {
        public ValueError(Guid code, string message)
        {
            Code = code;
            Message = message;
        }

        public Guid Code { get; }

        public string Message { get; }
    }
}
