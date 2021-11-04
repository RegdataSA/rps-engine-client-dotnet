using System;

namespace Regdata.RPS.Engine.Client.Model.Api.Error
{
    public class Error
    {
        public Error()
        {
        }

        public Error(string guid, string message)
            : this(new Guid(guid), message)
        {
        }

        public Error(Guid code, string message)
        {
            Code = code;
            Message = message;
        }

        public Guid Code { get; set; }

        public string Message { get; set; }
    }
}
