using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Regdata.RPS.Engine.Client.EngineContext;
using Regdata.RPS.Engine.Client.Instance;
using Regdata.RPS.Engine.Client.Value;

namespace Regdata.RPS.Engine.Client.Examples.Examples
{
    public class SimpleUsageExample : IHostedService
    {
        private readonly RPSEngine _rpsEngine;

        public SimpleUsageExample(RPSEngine rpsEngine)
        {
            _rpsEngine = rpsEngine;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            Console.ForegroundColor = ConsoleColor.DarkGreen;
            Console.WriteLine("--- Example of simple protection and deprotection ---");
            Console.ResetColor();

            // Manually creates admin rights context.
            var adminRightsContext = new Context
            {
                Evidences = new List<Evidence>
                {
                    new Evidence
                    {
                        Name = "Role",
                        Value = "Admin"
                    }
                }
            };

            // Manually creates protecting processing context.
            var protectProcessingContext = new ProcessingContext
            {
                Evidences = new List<Evidence>
                {
                    new Evidence
                    {
                        Name = "Action",
                        Value = "Protect"
                    }
                }
            };

            // Manually creates deprotecting processing context.
            var deprotectProcessingContext = new ProcessingContext
            {
                Evidences = new List<Evidence>
                {
                    new Evidence
                    {
                        Name = "Action",
                        Value = "Deprotect"
                    }
                }
            };

            // Different constructors with ability to specify Instance and Original value.
            var rawFirstName = new RPSValue(className: "User", propertyName: "FirstName", originalValue: "Jonny"); 
            var rawLastName = new RPSValue("User", "LastName", "Silverhand");
            var rawBirthDate = new RPSValue(new RPSInstance("User", "BirthDate"), "16.11.1988");

            var protectedFirstName = new RPSValue(className: "User", propertyName: "FirstName", originalValue: "n99toNMwdjjGtWs3SxkrxQ==");
            var protectedLastName = new RPSValue("User", "LastName", "FLGqfDklPngzYAD8066q40drM1jZYQzKktF1YO81A==");
            var protectedBirthDate = new RPSValue(new RPSInstance("User", "BirthDate"), "02.09.1961");

            // In this example we create a context with two different requests (protect and deprotect), BUT in single call to RPS Engine API.
            RequestContext requestContext = _rpsEngine
                .CreateContext()
                .WithRequest(
                    rpsValues: new[] { rawFirstName, rawLastName, rawBirthDate },
                    rightsContext: adminRightsContext,
                    processingContext: protectProcessingContext)
                .WithRequest(
                    rpsValues: new[] { protectedFirstName, protectedLastName, protectedBirthDate },
                    rightsContext: adminRightsContext,
                    processingContext: deprotectProcessingContext);

            // This method will do REST API call to RPS Engine API.
            await requestContext.TransformAsync();

            Console.WriteLine($"Raw fist name. Original: {rawFirstName.Original}. Transformed: {rawFirstName.Transformed}");
            Console.WriteLine($"Raw last name. Original: {rawLastName.Original}. Transformed: {rawLastName.Transformed}");
            Console.WriteLine($"Raw birth date. Original: {rawBirthDate.Original}. Transformed: {rawBirthDate.Transformed}");
            Console.WriteLine($"Protected first name. Original: {protectedFirstName.Original}. Transformed: {protectedFirstName.Transformed}");
            Console.WriteLine($"Protected last name. Original: {protectedLastName.Original}. Transformed: {protectedLastName.Transformed}");
            Console.WriteLine($"Protected birth date. Original: {protectedBirthDate.Original}. Transformed: {protectedBirthDate.Transformed}");

            Console.WriteLine();
        }

        public async Task StopAsync(CancellationToken cancellationToken) 
            => await Task.CompletedTask;
    }
}
