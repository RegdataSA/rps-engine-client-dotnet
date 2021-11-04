namespace Regdata.RPS.Engine.Client
{
    public class Evidence
    {
        public string Name { get; set; }

        public string Value { get; set; }
        
        public override int GetHashCode()
        {
            unchecked
            {
                return (Name != null ? (Name.GetHashCode() * 397) : 0) ^ (Value != null ? Value.GetHashCode() : 0);
            }
        }
    }
}
