using System;
using TheGreatC.Runtime;
using System.Collections.Generic;

namespace TheGreatC
{
    internal static class Startup
    {
        public static void Run()
        {
            ConfigureService();
        }

        // Startup Configuration Pipeline
        private static void ConfigureService()
        {
            Console.Title = "The Great C!";

            // Generate And Print Splash For Startup
            ShowSplash();

            // Start Core Service (For I/O Loop & Etc...)
            Core.Instance.Start();
        }

        // CLI Initiale Splash + Info
        private static void ShowSplash()
        {
            #region Splash
            // Get Program Version
            var programVersion = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString();
            var text = new List<string> {
                @" _______ _             _____                _      _____  ",
                @"|__   __| |           / ____|              | |    / ____| ",
                @"   | |  | |__   ___  | |  __ _ __ ___  __ _| |_  | |      ",
                @"   | |  | '_ \ / _ \ | | |_ | '__/ _ \/ _` | __| | |      ",
                @"   | |  | | | |  __/ | |__| | | |  __/ (_| | |_  | |____  ",
                @"   |_|  |_| |_|\___|  \_____|_|  \___|\__,_|\__|  \_____| ",
                @"__________________________________________________________",
                @"                                                          ",
                @"                  An Extensible C# CLI                    ",
                $@"               Version {programVersion}                  ",
                @"                                                          "
            };

            foreach (var line in text)
            {
                Translator.Instance.WriteToConsole(WrittingFormatType.Message, line);
            }
            #endregion
        }
    }
}
