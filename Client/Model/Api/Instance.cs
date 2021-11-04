namespace Regdata.RPS.Engine.Client.Model.Api
{
    public class Instance
    {
        public Context.Context LoggingContext { get; set; }

        public Context.Context DependencyContext { get; set; }

        public string ClassName { get; set; }

        public string PropertyName { get; set; }
        
        public string Value { get; set; }

        public ValueError Error { get; set; }
    }
}
