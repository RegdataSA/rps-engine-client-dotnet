using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Regdata.RPS.Engine.Client.Instance;
using Regdata.RPS.Engine.Client.Value;

namespace Regdata.RPS.Engine.Client.Examples.Examples
{
    public class ContextsProvidedByResolverExample : IHostedService
    {
        private readonly RPSEngine _rpsEngine;

        public ContextsProvidedByResolverExample(RPSEngine rpsEngine)
        {
            _rpsEngine = rpsEngine;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            Console.ForegroundColor = ConsoleColor.DarkGreen;
            Console.WriteLine("--- Example with rights and processing contexts provided by abstract resolver ---");
            Console.WriteLine("1. To use separate JSON files (RightsContexts.json and ProcessingContext.json):");
            Console.WriteLine("Uncomment line at Program.cs: 'services.AddRPSEngineContextJsonFileProvider(...);'");
            Console.WriteLine("2. To use Rights and Processing contexts from configuration file (appsettings.json):");
            Console.WriteLine("Uncomment line at Program.cs: 'services.AddRPSEngineContextConfigurationProvider();'");
            Console.ResetColor();

            // Different constructors with ability to specify Instance and Original value.
            var rawFirstName = new RPSValue(className: "User", propertyName: "FirstName", originalValue: "Jonny");
            var rawLastName = new RPSValue("User", "LastName", "Silverhand");
            var rawBirthDate = new RPSValue(new RPSInstance("User", "BirthDate"), "16.11.1988");

            var protectedFirstName = new RPSValue(className: "User", propertyName: "FirstName", originalValue: "n99toNMwdjjGtWs3SxkrxQ==");
            var protectedLastName = new RPSValue("User", "LastName", "FLGqfDklPngzYAD8066q40drM1jZYQzKktF1YO81A==");
            var protectedBirthDate = new RPSValue(new RPSInstance("User", "BirthDate"), "02.09.1961");

            // In this example we create a two different contexts (protect and deprotect)
            // with a single request inside each, it will do a two call to RPS Engine API.
            RequestContext protectRequestContext = _rpsEngine
                .CreateContext()
                .WithRequest(
                    rpsValues: new[] {protectedFirstName, protectedLastName, protectedBirthDate},
                    rightsContextName: "Admin",
                    processingContextName: "Protect");

            RequestContext deprotectRequestContext = _rpsEngine
                .CreateContext()
                .WithRequest(
                    rpsValues: new[] {protectedFirstName, protectedLastName, protectedBirthDate},
                    rightsContextName: "Admin",
                    processingContextName: "Deprotect");

            // This methods will do REST API calls to RPS Engine API.
            await protectRequestContext.TransformAsync();
            await deprotectRequestContext.TransformAsync();

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