using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Regdata.RPS.Engine.Client.Instance;
using Regdata.RPS.Engine.Client.Value;

namespace Regdata.RPS.Engine.Client.Examples.Examples
{
    class UsageWithDependenciesExample : IHostedService
    {
        private readonly RPSEngine _rpsEngine;

        public UsageWithDependenciesExample(RPSEngine rpsEngine)
        {
            _rpsEngine = rpsEngine;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            Console.ForegroundColor = ConsoleColor.DarkGreen;
            Console.WriteLine("--- Example of protection with dependencies ---");
            Console.ResetColor();

            // Different constructors with ability to specify Instance, Original value and Dependencies.
            var paymentDate = new RPSValue(className: "Payment", 
                propertyName: "Date", 
                originalValue: "02.11.2021", 
                dependencies: new Dictionary<string, string>
                {
                    { "min", "01.10.2021" }, 
                    { "max", "02.11.2021" }
                });

            var paymentAmount = new RPSValue(new RPSInstance("Payment", "Amount"), "999");

            // This method will do REST API call to RPS Engine API.
            await _rpsEngine.TransformAsync(
                    rpsValues: new[] { paymentDate, paymentAmount },
                    rightsContextName: "Admin",
                    processingContextName: "Protect");

            Console.WriteLine($"Payment date. Original: {paymentDate.Original}. Transformed: {paymentDate.Transformed}");
            Console.WriteLine($"Payment amount. Original: {paymentAmount.Original}. Transformed: {paymentAmount.Transformed}");

            Console.WriteLine();
        }

        public async Task StopAsync(CancellationToken cancellationToken)
            => await Task.CompletedTask;
    }
}
