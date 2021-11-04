using System;

namespace Regdata.RPS.Engine.Client
{
    public class EngineClientOptions
    {
        private static readonly TimeSpan DefaultTimeout = TimeSpan.FromMinutes(1);

        public string EngineHostName { get; set; }

        public string IdentityServerHostName { get; set; }

        public string ApiKey { get; set; }

        public string SecretKey { get; set; }

        public TimeSpan? Timeout { get; set; } = DefaultTimeout;
    }
}
