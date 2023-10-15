using System;
using System.Collections.Generic;
using TheGreatC.Domain;
using TheGreatC.Domain.Models;

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
                        Interpreter.WriteToConsole(
                            result.Response.Message.Contains("Not Found")
                                ? WrittingFormatType.NotFound
                                : WrittingFormatType.Error, result.Response.Message);

                        // If Error Occured With Object (Message Or Etc...)
                        if (result.Result != null)
                        {
                            switch (result.Result)
                            {
                                case List<string> _:
                                    foreach (var line in (List<string>)result.Result)
                                    {
                                        Interpreter.WriteToConsole(WrittingFormatType.None,
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
                                Interpreter.WriteToConsole(WrittingFormatType.Message,
                                    result.Response.Message);
                                break;
                            case List<string> _:
                                foreach (var line in (List<string>)result.Result)
                                {
                                    Interpreter.WriteToConsole(WrittingFormatType.Message,
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
                    Interpreter.WriteToConsole(WrittingFormatType.Error, ex.Message);
                }
            }
        }
    }
}