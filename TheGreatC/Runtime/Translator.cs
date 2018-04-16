using System;
using System.Linq;
using TheGreatC.Domain;
using System.Reflection;
using TheGreatC.Domain.Models;
using System.Collections.Generic;
using TheGreatC.Domain.ViewModels;
using TheGreatC.Common.Configuration;

namespace TheGreatC.Runtime
{
    public class Translator : CommandFactory
    {
        private const string ReadPrompt = "-> ";

        protected new static readonly string CommandNameSpace = Properties.CommandsNamespace;

        // Output Writing Mehtod
        public enum WrittingFormatType
        {
            Success = 0,
            Message = 1,
            Error = 2,
            NotFound = 3,
            WentWrong = 4,
            Warning = 5,
            None = 6
        }

        public static CommandResult Execute(Command command)
        {
            // Validate the command name:
            if (!CommandLibraries.ContainsKey(command.LibraryClassName))
            {
                return new CommandResult()
                {
                    Result = null,
                    Response = new CommandResponse()
                    {
                        Message = "Command Library Was Not Found! :(",
                        IsSuccessful = false
                    }
                };

            }
            var methodDictionary = CommandLibraries[command.LibraryClassName];
            if (!methodDictionary.ContainsKey(command.Name))
            {
                // Check For Similar Commands To Offer
                var similarCommands = methodDictionary.Where(c => c.Key.ToLower().Contains(command.Name.ToLower())).ToList().Take(5);
                var similarCommandsMessage = new List<string> { "Similar Commands: ", "---------" };
                var keyValuePairs = similarCommands as IList<KeyValuePair<string, IEnumerable<ParameterInfo>>> ?? similarCommands.ToList();

                if (!keyValuePairs.Any())
                {
                    return new CommandResult()
                    {
                        Result = null,
                        Response = new CommandResponse()
                        {
                            Message = "Command Was Not Found! :(",
                            IsSuccessful = false
                        }
                    };
                }


                similarCommandsMessage.AddRange(keyValuePairs.Select(similarCommand => similarCommand.Key));
                return new CommandResult()
                {
                    Result = similarCommandsMessage,
                    Response = new CommandResponse()
                    {
                        Message = "Command Was Not Found! :(",
                        IsSuccessful = false
                    }
                };
            }

            // Make sure the corret number of required arguments are provided:
            // -------------------------------------------------------------------------------

            var methodParameterValueList = new List<object>();
            IEnumerable<ParameterInfo> paramInfoList = methodDictionary[command.Name].ToList();

            // Validate proper # of required arguments provided. Some may be optional:
            var requiredParams = paramInfoList.Where(p => p.IsOptional == false);
            var optionalParams = paramInfoList.Where(p => p.IsOptional);
            var requiredParamsInfos = requiredParams as ParameterInfo[] ?? requiredParams.ToArray();
            var optionalParamsInfos = optionalParams as ParameterInfo[] ?? optionalParams.ToArray();
            var commandArgs = new CommandArgumentsDetail()
            {
                OptionalArgs = optionalParamsInfos.Count(),
                ProvidedArgs = command.Arguments.Count(),
                RequiredArgs = requiredParamsInfos.Count()
            };

            if (commandArgs.RequiredArgs > commandArgs.ProvidedArgs)
            {
                var missingRequiredArgs = requiredParamsInfos.Select(x => x.Name).ToList();

                var missingArgsMessage = commandArgs.RequiredArgs >= 2 ?
                    $"Error: 'Missing Required Arguments For Command Found' - {commandArgs.RequiredArgs} Required - Arguments:" : $"Error: 'Missing Required Argument For Command Found' - {commandArgs.RequiredArgs} Required - Argument:";
                for (var i = 0; i < missingRequiredArgs.Count; i++)
                {
                    if (i == 0)
                    {
                        missingArgsMessage += $" {missingRequiredArgs[i]}:{requiredParamsInfos[i].ParameterType.Name}";
                    }
                    else
                    {
                        missingArgsMessage += $", {missingRequiredArgs[i]}:{requiredParamsInfos[i].ParameterType.Name}";
                    }
                }

                return new CommandResult()
                {
                    Result = null,
                    Response = new CommandResponse()
                    {
                        Message = missingArgsMessage,
                        IsSuccessful = false
                    }
                };
            }

            // Warn User About Optional Argument(s)
            if (commandArgs.OptionalArgs + commandArgs.RequiredArgs > commandArgs.ProvidedArgs)
            {
                var missingOptionalArgs = optionalParamsInfos.Select(x => x.Name).ToList();
                var missingOptionalMessage = commandArgs.OptionalArgs >= 2 ?
                    $"Warning: 'Missing Optional Arguments For Command Found' - {commandArgs.OptionalArgs} Optional - Arguments:" : $"Warning: 'Missing Optional Argument For Command Found' - {commandArgs.OptionalArgs} Optional - Argument:";

                for (var i = 0; i < missingOptionalArgs.Count; i++)
                {
                    if (i == 0)
                    {
                        missingOptionalMessage += $" {missingOptionalArgs[i]}:{optionalParamsInfos[i].ParameterType.Name} - DefaultValue:'{optionalParamsInfos[i].RawDefaultValue}'";
                    }
                    else
                    {
                        missingOptionalMessage += $", {missingOptionalArgs[i]}:{optionalParamsInfos[i].ParameterType.Name} - DefaultValue:'{optionalParamsInfos[i].RawDefaultValue}'";
                    }
                }

                WriteToConsole(WrittingFormatType.Warning, missingOptionalMessage);
            }

            // Make sure all arguments are coerced to the proper type, and that there is a 
            // value for every emthod parameter. The InvokeMember method fails if the number 
            // of arguments provided does not match the number of parameters in the 
            // method signature, even if some are optional:
            // -------------------------------------------------------------------------------

            if (paramInfoList.Any())
            {
                // Populate the list with default values:
                methodParameterValueList.AddRange(paramInfoList.Select(param => param.DefaultValue));

                // Now walk through all the arguments passed from the console and assign 
                // accordingly. Any optional arguments not provided have already been set to 
                // the default specified by the method signature:
                for (var i = 0; i < command.Arguments.Count(); i++)
                {
                    var methodParam = paramInfoList.ElementAt(i);
                    var typeRequired = methodParam.ParameterType;
                    try
                    {
                        // Coming from the Console, all of our arguments are passed in as 
                        // strings. Coerce to the type to match the method paramter:
                        var value = CoerceArgument(typeRequired, command.Arguments.ElementAt(i));
                        methodParameterValueList.RemoveAt(i);
                        methodParameterValueList.Insert(i, value);
                    }
                    catch (ArgumentException)
                    {
                        var argumentName = methodParam.Name;
                        var argumentTypeName = typeRequired.Name;
                        var message =
                            $"The Value Passed For Argument '{argumentName}' Cannot Be Parsed To Type '{argumentTypeName}'.";
                        throw new ArgumentException(message);
                    }
                }
            }

            // Set up to invoke the method using reflection:
            // -------------------------------------------------------------------------------

            #region If Commands Embeded In Main Project

            //Assembly current = typeof(Program).Assembly;
            //// Need the full Namespace for this:
            //Type commandLibaryClass =
            //    current.GetType(_CommandNamespace + "." + command.LibraryClassName); 

            #endregion

            #region If Commands Not Embeded In Main Project

            var current = GetAssemblyByName(CommandNameSpace);
            var commandLibaryClass =
                current.GetType(CommandNameSpace + "." + command.LibraryClassName);

            #endregion


            object[] inputArgs = null;
            if (methodParameterValueList.Count > 0)
            {
                inputArgs = methodParameterValueList.ToArray();
            }

            var typeInfo = commandLibaryClass;

            // This will throw if the number of arguments provided does not match the number 
            // required by the method signature, even if some are optional:
            try
            {
                var result = typeInfo.InvokeMember(
                    command.Name,
                    BindingFlags.InvokeMethod | BindingFlags.Static | BindingFlags.Public,
                    null, null, inputArgs);

                return new CommandResult()
                {
                    Result = result,
                    Response = new CommandResponse()
                    {
                        Message = "",
                        IsSuccessful = true
                    }
                };
            }
            catch (TargetInvocationException ex)
            {
                if (ex.InnerException != null)
                    throw ex.InnerException;

                throw;
            }

        }

        public static string ReadFromConsole(string promptMessage = "")
        {
            // Show a prompt, and get input:
            Console.Write(ReadPrompt + promptMessage);
            return Console.ReadLine();
        }

        public static void WriteToConsole(WrittingFormatType writingFormat, string message)
        {
            if (message.Length <= 0) return;

            switch (writingFormat)
            {
                case WrittingFormatType.Message:
                    Console.SetCursorPosition((Console.WindowWidth - message.Length) / 2, Console.CursorTop);
                    Console.WriteLine(message);
                    break;

                case WrittingFormatType.Error:
                    Console.WriteLine("\n");
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.SetCursorPosition((Console.WindowWidth - message.Length) / 2, Console.CursorTop);
                    Console.WriteLine(message);
                    Console.WriteLine("\n");
                    // Annoying Beep Sound! ψ(｀∇´)ψ
                    //Console.Beep(1500, 250); 
                    Console.ResetColor();
                    break;

                case WrittingFormatType.NotFound:
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
                        WriteToConsole(WrittingFormatType.Message, line);
                    }

                    // Log Error
                    WriteToConsole(WrittingFormatType.Error, message);
                    break;

                case WrittingFormatType.Success:
                    break;

                case WrittingFormatType.WentWrong:
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
                        WriteToConsole(WrittingFormatType.Message, line);
                    }

                    // Log Error
                    WriteToConsole(WrittingFormatType.Error, message);
                    break;

                case WrittingFormatType.Warning:
                    Console.WriteLine("\n");
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.SetCursorPosition((Console.WindowWidth - message.Length) / 2, Console.CursorTop);
                    Console.WriteLine(message);
                    Console.WriteLine("\n");
                    Console.ResetColor();
                    break;

                case WrittingFormatType.None:
                    Console.WriteLine("\t" + message);
                    break;

                default:
                    throw new ArgumentOutOfRangeException(nameof(writingFormat), writingFormat, null);
            }
        }

        private static object CoerceArgument(Type requiredType, string inputValue)
        {
            var requiredTypeCode = Type.GetTypeCode(requiredType);
            var exceptionMessage =
                $"Cannnot Coerce The Input Argument {inputValue} To Required Type {requiredType.Name}";

            object result = null;
            switch (requiredTypeCode)
            {
                case TypeCode.String:
                    result = inputValue;
                    break;

                case TypeCode.Int16:
                    if (short.TryParse(inputValue, out var number16))
                    {
                        result = number16;
                    }
                    else
                    {
                        throw new ArgumentException(exceptionMessage);
                    }
                    break;

                case TypeCode.Int32:
                    if (int.TryParse(inputValue, out var number32))
                    {
                        result = number32;
                    }
                    else
                    {
                        throw new ArgumentException(exceptionMessage);
                    }
                    break;

                case TypeCode.Int64:
                    if (Int64.TryParse(inputValue, out var number64))
                    {
                        result = number64;
                    }
                    else
                    {
                        throw new ArgumentException(exceptionMessage);
                    }
                    break;

                case TypeCode.Boolean:
                    if (bool.TryParse(inputValue, out var trueFalse))
                    {
                        result = trueFalse;
                    }
                    else
                    {
                        throw new ArgumentException(exceptionMessage);
                    }
                    break;

                case TypeCode.Byte:
                    if (byte.TryParse(inputValue, out var byteValue))
                    {
                        result = byteValue;
                    }
                    else
                    {
                        throw new ArgumentException(exceptionMessage);
                    }
                    break;

                case TypeCode.Char:
                    if (char.TryParse(inputValue, out var charValue))
                    {
                        result = charValue;
                    }
                    else
                    {
                        throw new ArgumentException(exceptionMessage);
                    }
                    break;

                case TypeCode.DateTime:
                    if (DateTime.TryParse(inputValue, out var dateValue))
                    {
                        result = dateValue;
                    }
                    else
                    {
                        throw new ArgumentException(exceptionMessage);
                    }
                    break;

                case TypeCode.Decimal:
                    if (decimal.TryParse(inputValue, out var decimalValue))
                    {
                        result = decimalValue;
                    }
                    else
                    {
                        throw new ArgumentException(exceptionMessage);
                    }
                    break;

                case TypeCode.Double:
                    if (double.TryParse(inputValue, out var doubleValue))
                    {
                        result = doubleValue;
                    }
                    else
                    {
                        throw new ArgumentException(exceptionMessage);
                    }
                    break;

                case TypeCode.Single:
                    if (float.TryParse(inputValue, out var singleValue))
                    {
                        result = singleValue;
                    }
                    else
                    {
                        throw new ArgumentException(exceptionMessage);
                    }
                    break;

                case TypeCode.UInt16:
                    if (ushort.TryParse(inputValue, out var uInt16Value))
                    {
                        result = uInt16Value;
                    }
                    else
                    {
                        throw new ArgumentException(exceptionMessage);
                    }
                    break;

                case TypeCode.UInt32:
                    if (uint.TryParse(inputValue, out var uInt32Value))
                    {
                        result = uInt32Value;
                    }
                    else
                    {
                        throw new ArgumentException(exceptionMessage);
                    }
                    break;

                case TypeCode.UInt64:
                    if (ulong.TryParse(inputValue, out var uInt64Value))
                    {
                        result = uInt64Value;
                    }
                    else
                    {
                        throw new ArgumentException(exceptionMessage);
                    }
                    break;

                default:
                    throw new ArgumentException(exceptionMessage);
            }

            return result;
        }

    }
}