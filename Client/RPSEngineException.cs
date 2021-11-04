using System;
using Regdata.RPS.Engine.Client.Model.Api.Error;

namespace Regdata.RPS.Engine.Client
{
    public class RPSEngineException : Exception
    {
        public RPSEngineException()
        {
        }

        public RPSEngineException(string message)
            : base(message)
        {
        }

        public RPSEngineException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        public RPSEngineException(string message, Error error)
            : base(message)
            => Error = error;

        public Error Error { get; set; }
    }
}
