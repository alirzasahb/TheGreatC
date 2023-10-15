using Microsoft.Extensions.Configuration;
using Spectre.Console;
using System;
using System.IO;

namespace TheGreatC.Common
{
    public static class ConfigurationManager
    {
        public static IConfigurationRoot SharedConfigurations { get; private set; }

        public static void Build()
        {
            AnsiConsole.Status()
            .Start("Initializing Configurations...", ctx =>
            {
                ctx.Spinner(Spinner.Known.Dots2);

                // set up configuration source

                var configurationBuilder = new ConfigurationBuilder()
                    .SetBasePath(Path.Combine(AppContext.BaseDirectory))
                    .AddJsonFile("sharedsettings.json", true);

                SharedConfigurations = configurationBuilder.Build();

                ctx.SpinnerStyle(Style.Parse("green"));
                ctx.Status("[green]Loaded Configurations Successfully.[/]");
            });
        }
    }
}