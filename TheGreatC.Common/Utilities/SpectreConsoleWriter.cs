using Spectre.Console;

namespace TheGreatC.Common.Utilities
{
    public static class SpectreConsoleWriter
    {
        // Output Writing Mehtod
        public enum SpectreWritingType
        {
            Info = 0,
            Figlet,
            Success,
            Warning
        }

        public static void WriteException(System.Exception exception)
        {
            AnsiConsole.WriteException(exception,
                            ExceptionFormats.ShortenPaths | ExceptionFormats.ShortenTypes |
                            ExceptionFormats.ShortenMethods | ExceptionFormats.ShowLinks);
        }

        public static void WriteVersion()
        {
            var font = FigletFont.Load("Figlet/Font/Colossal.flf");

            AnsiConsole.Write(
                new FigletText(font, "TheGreatC")
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
                    break;
                case SpectreWritingType.Figlet:
                    {
                        AnsiConsole.Write(
                        new FigletText(message)
                            .Centered()
                            );

                        break;
                    }
                case SpectreWritingType.Success:
                    break;
                case SpectreWritingType.Warning:
                    break;
            }
        }

    }
}
