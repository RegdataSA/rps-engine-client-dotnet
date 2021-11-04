namespace Regdata.RPS.Engine.Client.Model.Api.Context
{
    public class Evidence
    {
        public string Name { get; set; }
        
        public string Value { get; set; }
        
        public override int GetHashCode()
        {
            unchecked
            {
                return ((Name != null ? Name.GetHashCode() : 0) * 397) ^ (Value != null ? Value.GetHashCode() : 0);
            }
        }
    }
}
