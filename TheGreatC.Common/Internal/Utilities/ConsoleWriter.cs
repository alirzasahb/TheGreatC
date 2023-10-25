using System;
using System.Collections.Generic;

namespace TheGreatC.Common.Internal.Utilities
{
    public static class ConsoleWriter
    {
        // Output Writing Mehtod
        public enum ConsoleWritingTypes
        {
            Success = 0,
            Message = 1,
            Error = 2,
            NotFound = 3,
            WentWrong = 4,
            Warning = 5,
            None = 6
        }

        public static void Write(ConsoleWritingTypes writingFormat, string message)
        {
            while (true)
            {
                if (message.Length <= 0) return;

                switch (writingFormat)
                {
                    case ConsoleWritingTypes.Message:
                        Console.SetCursorPosition((Console.WindowWidth - message.Length) / 2, Console.CursorTop);
                        Console.WriteLine(message);
                        break;

                    case ConsoleWritingTypes.Error:
                        Console.WriteLine("\n");
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.SetCursorPosition((Console.WindowWidth - message.Length) / 2, Console.CursorTop);
                        Console.WriteLine(message);
                        Console.WriteLine("\n");
                        // Annoying Beep Sound! ψ(｀∇´)ψ
                        //Console.Beep(1500, 250); 
                        Console.ResetColor();
                        break;

                    case ConsoleWritingTypes.NotFound:
                        // Log ASCII Art
                        var text = new List<string>
                        {
                            @"    .    _    +     .  ______   .          .     '      .            '+",
                            @"  (      /|\      _   _|      \___   .   +    '    .         *         ",
                            @"    /\  ||||| .  | | |   | |      |       .    '                 .    '",
                            @" __||||_|||||____| |_|_____________\___________________________________",
                            @" . |||| |||||  /\   _____      _____  .   .       .            .      .",
                            @"  . \|`-'|||| ||||    __________            .                          ",
                            @"     \__ |||| ||||      .          .     .     .        -         .   .",
                            @"  __    ||||`-'|||  .       .    __________                            ",
                            @" .    . |||| ___/  ___________             .                           ",
                            @" _   ___|||||__  _           .          _                              ",
                            @"      _ `---'    .   .    .   _   .   .    .                           ",
                            @" _  ^      .  -    .    -    .       -    .    .  .     -   .    .    -"
                        };

                        foreach (var line in text)
                        {
                            Write(ConsoleWritingTypes.Message, line);
                        }

                        // Log Error
                        writingFormat = ConsoleWritingTypes.Error;
                        continue;

                    case ConsoleWritingTypes.Success:
                        break;

                    case ConsoleWritingTypes.WentWrong:
                        // Log ASCII Art
                        text = new List<string>
                        {
                            @"                 _                 (Beep...Beep...)                   ",
                            @"                /\\               ( Looks Like Something Went Wrong. )",
                            @"                \ \\  \__/ \__/  /                                    ",
                            @"                 \ \\ (oo) (oo) /                                     ",
                            @"                  \_\\/~~\_/~~\_                                      ",
                            @"                 _.-~===========~-._                                  ",
                            @"                (___/_______________)                                 ",
                            @"                   /  \_______/                                       ",
                        };

                        foreach (var line in text)
                        {
                            Write(ConsoleWritingTypes.Message, line);
                        }

                        // Log Error
                        writingFormat = ConsoleWritingTypes.Error;
                        continue;

                    case ConsoleWritingTypes.Warning:
                        Console.WriteLine("\n");
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.SetCursorPosition((Console.WindowWidth - message.Length) / 2, Console.CursorTop);
                        Console.WriteLine(message);
                        Console.WriteLine("\n");
                        Console.ResetColor();
                        break;

                    case ConsoleWritingTypes.None:
                        Console.WriteLine("\t" + message);
                        break;

                    default:
                        throw new ArgumentOutOfRangeException(nameof(writingFormat), writingFormat, null);
                }

                break;
            }
        }

    }
}
