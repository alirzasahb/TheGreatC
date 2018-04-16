using System;
using TheGreatC.Domain.Models;
using System.Collections.Generic;
using TheGreatC.Common.Configuration;
using TheGreatC.Domain.ViewModels;

namespace TheGreatC.Runtime
{
    public class Core : Translator
    {
        public static void Start()
        {
            // Create List Of Installed Commands
            LoadInstalledCommands();
            // I/O Loop
            while (true)
            {
                var consoleInput = ReadFromConsole();
                if (string.IsNullOrWhiteSpace(consoleInput)) continue;
                try
                {
                    // Create a ConsoleCommand instance:
                    var cmd = new Command(consoleInput);

                    // Execute the command
                    var result = Execute(cmd);

                    // If Error Occured
                    if (!result.Response.IsSuccessful)
                    {
                        WriteToConsole(
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
                                        WriteToConsole(WrittingFormatType.None, line);
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
                                WriteToConsole(WrittingFormatType.Message, result.Response.Message);
                                break;
                            case List<string> _:
                                foreach (var line in (List<string>)result.Result)
                                {
                                    WriteToConsole(WrittingFormatType.Message, line);
                                }
                                break;
                        }
                    }
                }
                catch (Exception ex)
                {
                    // Something Went Wrong - Write out the error
                    WriteToConsole(WrittingFormatType.Error, ex.Message);
                    // Log Error
                    if (Properties.LogFeature)
                        Logger.Log(ex.Message);
                }

            }
        }
    }
}
