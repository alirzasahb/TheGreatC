using System;
using System.Collections.Generic;
using TheGreatC.Common.Internal.Utilities;
using TheGreatC.Domain;
using TheGreatC.Domain.Models;

namespace TheGreatC.Runtime
{
    public static class Core
    {
        public static void Start()
        {
            // ToDo: Port All Writers To SpectreConsoleWriter

            // Console & PreStart Required Configurations
            Configurator.StartUp();

            // Create List Of Installed Commands
            Commander.LoadCommands();
            // I/O Loop
            while (true)
            {
                var consoleInput = Interpreter.ReadFromConsole();
                if (string.IsNullOrWhiteSpace(consoleInput)) continue;
                try
                {
                    // Create A ConsoleCommand instance:
                    var cmd = new Command(consoleInput);

                    // Execute the command
                    var result = Interpreter.Execute(cmd);

                    // If Error Occured
                    if (!result.Response.IsSuccessful)
                    {
                        if (result.Response.IsNotFound)
                            ConsoleWriter.Write(ConsoleWriter.ConsoleWritingTypes.NotFound, result.Response.Message);
                        else
                            ConsoleWriter.Write(ConsoleWriter.ConsoleWritingTypes.Error, result.Response.Message);

                        // If Error Occured With Object (Message Or Etc...)
                        if (result.Result != null)
                        {
                            switch (result.Result)
                            {
                                case List<string> _:
                                    foreach (var line in (List<string>)result.Result)
                                    {
                                        ConsoleWriter.Write(ConsoleWriter.ConsoleWritingTypes.None,
                                            line);
                                    }

                                    break;
                            }
                        }
                    }
                    else
                    {
                        // Write out the result
                        switch (result.Result)
                        {
                            case string _:
                                ConsoleWriter.Write(ConsoleWriter.ConsoleWritingTypes.Message,
                                        result.Response.Message);
                                break;
                            case List<string> _:
                                foreach (var line in (List<string>)result.Result)
                                {
                                    ConsoleWriter.Write(ConsoleWriter.ConsoleWritingTypes.Message,
                                        line);
                                }

                                break;
                        }
                    }
                }
                catch (Exception ex)
                {
                    SpectreConsoleWriter.WriteException(ex);
                }
            }
        }
    }
}