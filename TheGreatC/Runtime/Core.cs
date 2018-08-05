using System;
using TheGreatC.Domain;
using TheGreatC.Domain.Models;
using System.Collections.Generic;

namespace TheGreatC.Runtime
{
    public class Core
    {
        private static Core _instance;

        private Core()
        {
        }

        public static Core GetInstance()
        {
            return _instance ?? (_instance = new Core());
        }

        public void Start()
        {
            // Create List Of Installed Commands
            CommandFactory.GetInstance().LoadInstalledCommands();
            // I/O Loop
            while (true)
            {
                var consoleInput = Translator.GetInstance().ReadFromConsole();
                if (string.IsNullOrWhiteSpace(consoleInput)) continue;
                try
                {
                    // Create a ConsoleCommand instance:
                    var cmd = new Command(consoleInput);

                    // Execute the command
                    var result = Translator.GetInstance().Execute(cmd);

                    // If Error Occured
                    if (!result.Response.IsSuccessful)
                    {
                        Translator.GetInstance().WriteToConsole(
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
                                        Translator.GetInstance().WriteToConsole(WrittingFormatType.None,
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
                                Translator.GetInstance().WriteToConsole(WrittingFormatType.Message,
                                    result.Response.Message);
                                break;
                            case List<string> _:
                                foreach (var line in (List<string>)result.Result)
                                {
                                    Translator.GetInstance().WriteToConsole(WrittingFormatType.Message,
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
                    Translator.GetInstance().WriteToConsole(WrittingFormatType.Error, ex.Message);
                }
            }
        }
    }
}
