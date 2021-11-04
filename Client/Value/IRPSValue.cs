using System.Collections.Generic;
using Regdata.RPS.Engine.Client.Instance;

namespace Regdata.RPS.Engine.Client.Value
{
    public interface IRPSValue
    {
        IReadOnlyDictionary<string, string> Dependencies { get; }

        RPSInstance Instance { get; }

        string Value { get; set; }

        ValueError Error { get; set; }

        void AddDependency(string name, string value);

        void RemoveDependency(string name);
    }

    public interface IRPSValue<out T> : IRPSValue
    {
        T Original { get; }

        T Transformed { get; }
    }
}
