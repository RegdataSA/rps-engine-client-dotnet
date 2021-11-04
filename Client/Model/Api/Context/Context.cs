using System;
using System.Collections.Generic;
using System.Linq;

namespace Regdata.RPS.Engine.Client.Model.Api.Context
{
    public class Context
    {
        public Guid Guid { get; set; }
        
        public List<Evidence> Evidences { get; set; }

        public override int GetHashCode()
        {
            unchecked
            {
                // ReSharper disable once NonReadonlyMemberInGetHashCode
                return Evidences
                    .Select(v => v.GetHashCode())
                    .Aggregate(0, (total, next) => (total * 397) ^ next);
            }
        }
    }
}
