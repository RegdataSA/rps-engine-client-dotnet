using System.Collections.Generic;
using Regdata.RPS.Engine.Client.Instance;

namespace Regdata.RPS.Engine.Client.Value
{
    public abstract class RPSValueBase : IRPSValue
    {
        private readonly Dictionary<string, string> _dependencies = new Dictionary<string, string>();

        protected RPSValueBase(RPSInstance instance = null,
            IDictionary<string, string> dependencies = null)
        {
            Instance = instance;

            if (dependencies != null)
                AddDependencyRange(dependencies);
        }

        public IReadOnlyDictionary<string, string> Dependencies => _dependencies;

        public RPSInstance Instance { get; }

        public virtual string Value { get; set; }

        public ValueError Error { get; set; }

        public void AddDependency(string name, string value)
            => _dependencies[name] = value;

        public void AddDependencyRange(IDictionary<string, string> dependencies)
        {
            foreach (KeyValuePair<string, string> dependency in dependencies)
                AddDependency(dependency.Key, dependency.Value);
        }

        public void RemoveDependency(string name)
            => _dependencies.Remove(name);
    }
}
