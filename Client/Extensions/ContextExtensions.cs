using System;
using System.Collections.Generic;

namespace Regdata.RPS.Engine.Client.Extensions
{
    public static class ContextExtensions
    {
        public static Model.Api.Context.Context ToContext(this IReadOnlyDictionary<string, string> dictionary, Guid? contextGuid = default)
        {
            var context = new Model.Api.Context.Context
            {
                Evidences = new List<Model.Api.Context.Evidence>()
            };

            if (contextGuid.HasValue)
                context.Guid = contextGuid.Value;

            foreach (KeyValuePair<string, string> pair in dictionary)
                context.Evidences.Add(
                    new Model.Api.Context.Evidence {Name = pair.Key, Value = pair.Value});

            return context;
        }
    }
}
