using System;

namespace Regdata.RPS.Engine.Client.Model.Api
{
    public class ValueError
    {
        public ValueError()
        {
        }

        public ValueError(string guid, string message)
            : this(new Guid(guid), message)
        {
        }

        public ValueError(Guid code, string message)
        {
            Code = code;
            Message = message;
        }

        public Guid Code { get; set; }

        public string Message { get; set; }

        public static ValueError Create(string guid, string message) 
            => new ValueError(guid, message);
    }
}
