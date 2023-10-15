using System;
using System.Collections.Generic;
using TheGreatC.Domain;
using TheGreatC.Domain.Models;
using static TheGreatC.Common.Utilities.ConsoleWriter;

namespace TheGreatC.Runtime
{
    public static class Core
    {
        public static void Start()
        {
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
                        Write(
                            result.Response.Message.Contains("Not Found")
                                ? ConsoleWritingTypes.NotFound
                                : ConsoleWritingTypes.Error, result.Response.Message);

                        // If Error Occured With Object (Message Or Etc...)
                        if (result.Result != null)
                        {
                            switch (result.Result)
                            {
                                case List<string> _:
                                    foreach (var line in (List<string>)result.Result)
                                    {
                                        Write(ConsoleWritingTypes.None,
                                            line);
                                    }

                                    break;
                            }
                        }
                    }
                    else
                    {
                        // ToDo: Handle Different Types Of Command Output
                        // Write out the result
                        switch (result.Result)
                        {
                            case string _:
                                Write(ConsoleWritingTypes.Message,
                                    result.Response.Message);
                                break;
                            case List<string> _:
                                foreach (var line in (List<string>)result.Result)
                                {
                                    Write(ConsoleWritingTypes.Message,
                                        line);
                                }

                                break;
                        }
                    }
                }
                // ToDo: Handle Errors
                catch (Exception ex)
                {
                    // Something Went Wrong - Write out the error
                    Write(ConsoleWritingTypes.Error, ex.Message);
                }
            }
        }
    }
}