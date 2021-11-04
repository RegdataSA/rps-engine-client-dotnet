namespace Regdata.RPS.Engine.Client.Instance
{
    public class RPSInstance
    {
        public static readonly RPSInstance Empty = new RPSInstance();

        public RPSInstance()
        {
        }

        public RPSInstance(string className, string propertyName)
        {
            ClassName = className;
            PropertyName = propertyName;
        }

        public string ClassName { get; set; }

        public string PropertyName { get; set; }

        public override bool Equals(object obj)
            => obj is RPSInstance mapping && this == mapping;

        public override int GetHashCode()
        {
            unchecked
            {
                return (ClassName.GetHashCode() * 397) ^ PropertyName.GetHashCode();
            }
        }

        public static bool operator ==(RPSInstance x, RPSInstance y)
            => x?.ClassName == y?.ClassName && x?.PropertyName == y?.PropertyName;

        public static bool operator !=(RPSInstance x, RPSInstance y)
            => !(x == y);
    }
}
