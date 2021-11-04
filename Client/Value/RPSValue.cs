using System.Collections.Generic;
using Regdata.RPS.Engine.Client.Instance;

namespace Regdata.RPS.Engine.Client.Value
{
    public class RPSValue : RPSValueBase, IRPSValue<string>
    {
        public RPSValue(RPSInstance instance = null, 
            string originalValue = default,
            IDictionary<string, string> dependencies = null)
            : base(instance: instance, dependencies: dependencies)
        {
            Original = originalValue;
        }

        public RPSValue(string className,
            string propertyName,
            string originalValue = default,
            IDictionary<string, string> dependencies = null)
            : this(instance: new RPSInstance(className, propertyName), 
                originalValue: 
                originalValue, dependencies: dependencies)
        {
        }

        public override string Value
        {
            get => Original;
            set => Transformed = value;
        }

        public virtual string Original { get; }

        public virtual string Transformed { get; protected set; }
    }

    public class RPSValue<TTarget> : RPSValue
    {
        public RPSValue(TTarget target, 
            RPSInstance instance = null, 
            string originalValue = default,
            IDictionary<string, string> dependencies = null)
            : base(instance: instance, 
                originalValue: originalValue, 
                dependencies: dependencies)
        {
            Target = target;
        }

        public RPSValue(TTarget target,
            string className,
            string propertyName,
            string originalValue = default,
            IDictionary<string, string> dependencies = null)
            : this(target, 
                instance: new RPSInstance(className, propertyName), 
                originalValue: originalValue, 
                dependencies: dependencies)
        {
        }

        public TTarget Target { get; set; }
    }
}
