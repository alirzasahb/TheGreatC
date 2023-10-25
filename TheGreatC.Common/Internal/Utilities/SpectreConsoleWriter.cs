using Spectre.Console;

namespace TheGreatC.Common.Internal.Utilities
{
    public static class SpectreConsoleWriter
    {
        // Output Writing Mehtod
        public enum SpectreWritingType
        {
            Info = 0,
            Figlet,
            Success,
            Warning,
            NotFound,

        }

        public static void WriteException(System.Exception exception)
        {
            AnsiConsole.WriteException(exception,
                            ExceptionFormats.ShortenPaths | ExceptionFormats.ShortenTypes |
                            ExceptionFormats.ShortenMethods | ExceptionFormats.ShowLinks);
        }

        public static void WriteVersion()
        {
            var font = FigletFont.Load(ConfigurationManager.SharedConfigurations["Settings:Fonts:Figlet"]);

            AnsiConsole.Write(
                new FigletText(font, ConfigurationManager.SharedConfigurations["Settings:Title"])
                    .Centered()
                    );

            System.Reflection.Assembly assembly = System.Reflection.Assembly.GetExecutingAssembly();
            System.Diagnostics.FileVersionInfo fvi = System.Diagnostics.FileVersionInfo.GetVersionInfo(assembly.Location);
            string version = fvi.FileVersion;

            AnsiConsole.Write(
                new Markup($"Version: {version}")
                    .Centered()
                    );
        }

        public static void Write(SpectreWritingType writingFormat, string message)
        {
            switch (writingFormat)
            {
                case SpectreWritingType.Info:
                    {
                        AnsiConsole.MarkupLine($"{message}");
                        break;
                    }
                case SpectreWritingType.Figlet:
                    {
                        AnsiConsole.Write(
                            new FigletText(message)
                                .Centered()
                                );

                        break;
                    }
                case SpectreWritingType.Success:
                    {
                        AnsiConsole.MarkupLine($"[green]{message}[/]");
                        break;
                    }
                case SpectreWritingType.Warning:
                    {
                        AnsiConsole.MarkupLine($"[yellow]{message}[/]");
                        break;
                    }
                case SpectreWritingType.NotFound:
                    {
                        AnsiConsole.MarkupLine($"[red]{message}[/]");
                        break;
                    }
            }
        }

    }
}
