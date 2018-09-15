using System;
using System.IO;
using System.Text;
using Microsoft.Extensions.Configuration;

namespace TheGreatC.Common
{
    public static class ConfigurationManager
    {
        public static IConfigurationRoot SharedConfigurations { get; private set; }

        public static void Build()
        {
            Console.OutputEncoding = Encoding.UTF8;

            var environmentVariable = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

            if (string.IsNullOrWhiteSpace(environmentVariable))
                throw new ArgumentNullException($"Environment Not Found In ASPNETCORE_ENVIRONMENT");

            Console.WriteLine("Environment: {0}", environmentVariable);

            // Set up configuration sources

            var configurationBuilder = new ConfigurationBuilder()
                .SetBasePath(Path.Combine(AppContext.BaseDirectory))
                .AddJsonFile("sharedsettings.json", true);

            SharedConfigurations = configurationBuilder.Build();

            Console.WriteLine($"Commands Namespace: {SharedConfigurations["CommandsNameSpace"]}");
        }
    }
}