using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json.Linq;
using Regdata.RPS.Engine.Client.Instance;
using Regdata.RPS.Engine.Client.Value;

namespace Regdata.RPS.Engine.Client.Examples.Examples
{
    public class UsageWithRelatedObjectExample : IHostedService
    {
        private const string ExampleJsonFilePath = @"Data\ExampleOfJsonToProtect.json";
        private readonly RPSEngine _rpsEngine;

        public UsageWithRelatedObjectExample(RPSEngine rpsEngine)
        {
            _rpsEngine = rpsEngine;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            Console.ForegroundColor = ConsoleColor.DarkGreen;
            Console.WriteLine("--- Example of protection JSON file with related object ---");
            Console.ResetColor();

            var originalJson = await File.ReadAllTextAsync(ExampleJsonFilePath, cancellationToken);

            Console.WriteLine("Original JSON:");
            Console.WriteLine(originalJson);
            Console.WriteLine();

            JObject rootJObject = JObject.Parse(originalJson);

            var rpsValues = new List<RPSValue<JValue>>();

            foreach (JProperty jProperty in rootJObject.Properties())
            {
                string propertyName = jProperty.Name;

                JValue jValue = jProperty.Value as JValue;
                if (jValue == null)
                    continue;

                string propertyValue = jValue.Value<string>();
                
                // Attach to RPSValue - JValue as related object.
                var rpsValue = new RPSValue<JValue>(jValue, 
                    new RPSInstance("User", propertyName), 
                    propertyValue);

                rpsValues.Add(rpsValue);
            }
            
            RequestContext requestContext = _rpsEngine
                .CreateContext()
                .WithRequest(rpsValues: rpsValues, 
                    rightsContextName: "Admin", 
                    processingContextName: "Protect");
            
            await requestContext.TransformAsync();

            foreach (RPSValue<JValue> rpsValue in rpsValues)
            {
                // Example of usage related object (Target property) to insert back to JValue - protected value.
                rpsValue.Target.Value = rpsValue.Transformed;
            }

            string transformedJson = rootJObject.ToString();

            Console.WriteLine("Transformed JSON:");
            Console.WriteLine(transformedJson);
            Console.WriteLine();

            Console.WriteLine();
        }

        public async Task StopAsync(CancellationToken cancellationToken)
            => await Task.CompletedTask;
    }
}
