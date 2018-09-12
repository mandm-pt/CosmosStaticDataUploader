using CosmosStaticDataUploader.Config;
using Microsoft.Extensions.Configuration;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using static CosmosStaticDataUploader.Utils;

namespace CosmosStaticDataUploader
{
    internal class Program
    {
        private const string configFile = "appsettings.json";

        internal static Configurations Configurations { get; set; } = new Configurations();

        private static async Task Main(string[] args)
        {
#if DEBUG
            args = new[] { Directory.GetCurrentDirectory() + "\\Data", "local" };
#endif

            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile(configFile);
            builder.Build().Bind(Configurations);

            if (!ValidateInput(args)) return;

            Write($"Starting importing in 5s on Environment: ", ConsoleColor.White);
            WriteLine(args[1], ConsoleColor.Yellow);
            await Task.Delay(5 * 1000); // just to give time to user to cancel

            await DocumentsProcessor.Start(args[0], 
                Configurations.Environments.First(e => e.Name.Equals(args[1], StringComparison.InvariantCultureIgnoreCase)));

            WriteLine("Done! Press any key to exit", ConsoleColor.Green);
            Console.ReadKey();
        }

        private static bool ValidateInput(string[] args)
        {
            bool withErrors = true;
            if (args == null || args.Length != 2)
            {
                WriteLine($@"Usage:
    workingDir Env

    workingDir - Folder that contains the documents to upload.
    Env - Environment. Supported values are dependent on settings defined on {configFile} file.
", ConsoleColor.Yellow);
                return false;
            }

            if (!Directory.Exists(args[0]))
            {
                WriteLine("Provide the path to the folder that contains the documents to upload.", ConsoleColor.Red);
                withErrors = false;
            }

            if (!Configurations.Environments.Any(e => e.Name.Equals(args[1], StringComparison.InvariantCultureIgnoreCase)))
            {
                WriteLine($"The Environment specified '{args[1]}' isn't configured in '{configFile}'", ConsoleColor.Red);
                withErrors = false;
            }

            return withErrors;
        }
    }
}
