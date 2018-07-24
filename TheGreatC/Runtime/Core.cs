using System;
using TheGreatC.Domain;
using TheGreatC.Domain.Models;
using System.Collections.Generic;

namespace TheGreatC.Runtime
{
    public class Core
    {
        public static readonly Core Instance = new Core();

        private Core()
        {
        }

        public void Start()
        {
            // Create List Of Installed Commands
            CommandFactory.Instance.LoadInstalledCommands();
            // I/O Loop
            while (true)
            {
                var consoleInput = Translator.Instance.ReadFromConsole();
                if (string.IsNullOrWhiteSpace(consoleInput)) continue;
                try
                {
                    // Create a ConsoleCommand instance:
                    var cmd = new Command(consoleInput);

                    // Execute the command
                    var result = Translator.Instance.Execute(cmd);

                    // If Error Occured
                    if (!result.Response.IsSuccessful)
                    {
                        Translator.Instance.WriteToConsole(
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
                                        Translator.Instance.WriteToConsole(WrittingFormatType.None,
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
                                Translator.Instance.WriteToConsole(WrittingFormatType.Message,
                                    result.Response.Message);
                                break;
                            case List<string> _:
                                foreach (var line in (List<string>)result.Result)
                                {
                                    Translator.Instance.WriteToConsole(WrittingFormatType.Message,
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
                    Translator.Instance.WriteToConsole(WrittingFormatType.Error, ex.Message);
                }
            }
        }
    }
}
