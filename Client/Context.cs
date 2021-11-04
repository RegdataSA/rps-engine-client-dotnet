using System.Collections.Generic;
using System.Linq;

namespace Regdata.RPS.Engine.Client
{
    public class Context
    {
        public List<Evidence> Evidences { get; set; }

        public override int GetHashCode()
        {
            unchecked
            {
                // ReSharper disable once NonReadonlyMemberInGetHashCode
                return Evidences.Select(v => v.GetHashCode()).Aggregate((total, next) => (total * 397) ^ next);
            }
        }
    }
}
