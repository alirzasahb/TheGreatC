using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using TheGreatC.Common.Internal.Parser;
using TheGreatC.Domain;
using TheGreatC.Domain.DTOs;
using TheGreatC.Domain.Models;
using static TheGreatC.Common.Internal.Utilities.SpectreConsoleWriter;

namespace TheGreatC.Runtime
{
    public static class Interpreter
    {
        private static readonly string ReadPrompt = "\u2192";

        public static string ReadFromConsole()
        {
            // Show a prompt, and get input:
            Console.Write(ReadPrompt);
            return Console.ReadLine();
        }

        public static CommandResult Execute(Command command)
        {
            // Validate the command name:
            if (Commander.CommandLibraries.All(x => x.Item1 != command.LibraryClassName))
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

            var methodDictionary =
                Commander.CommandLibraries.First(x => x.Item1 == command.LibraryClassName);

            if (!methodDictionary.Item2.ContainsKey(command.Name))
            {
                // Check For Similar Commands From Same Library To Offer
                var similarCommands = methodDictionary.Item2
                    .Where(c => c.Key.ToLower().Contains(command.Name.ToLower()))
                    .ToList().Take(5);
                var similarCommandsMessage = new List<string> { "Similar Commands: ", "---------" };
                var keyValuePairs = similarCommands as IList<KeyValuePair<string, IEnumerable<ParameterInfo>>> ??
                                    similarCommands.ToList();

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
            IEnumerable<ParameterInfo> paramInfoList = methodDictionary.Item2[command.Name].ToList();

            // Validate proper # of required arguments provided. Some may be optional:
            var requiredParams = paramInfoList.Where(p => p.IsOptional == false);
            var optionalParams = paramInfoList.Where(p => p.IsOptional);
            var requiredParamsInfos = requiredParams as ParameterInfo[] ?? requiredParams.ToArray();
            var optionalParamsInfos = optionalParams as ParameterInfo[] ?? optionalParams.ToArray();
            var commandArgs = new CommandArgumentsDetail()
            {
                OptionalArgs = optionalParamsInfos.Length,
                ProvidedArgs = command.Arguments.Count(),
                RequiredArgs = requiredParamsInfos.Length
            };

            if (commandArgs.RequiredArgs > commandArgs.ProvidedArgs)
            {
                var missingRequiredArgs = requiredParamsInfos.Select(x => x.Name).ToList();

                var missingArgsMessage = commandArgs.RequiredArgs >= 2
                    ? $"Error: 'Missing Required Arguments For Command Found' - {commandArgs.RequiredArgs} Required - Arguments:"
                    : $"Error: 'Missing Required Argument For Command Found' - {commandArgs.RequiredArgs} Required - Argument:";
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
                var missingOptionalMessage = commandArgs.OptionalArgs >= 2
                    ? $"Warning: 'Missing Optional Arguments For Following Command' - {commandArgs.OptionalArgs} Optional - Arguments:"
                    : $"Warning: 'Missing Optional Argument For Following Command' - {commandArgs.OptionalArgs} Optional - Argument:";

                for (var i = 0; i < missingOptionalArgs.Count; i++)
                {
                    if (i == 0)
                    {
                        missingOptionalMessage +=
                            $" {missingOptionalArgs[i]}:{optionalParamsInfos[i].ParameterType.Name} - DefaultValue:'{optionalParamsInfos[i].RawDefaultValue}'";
                    }
                    else
                    {
                        missingOptionalMessage +=
                            $", {missingOptionalArgs[i]}:{optionalParamsInfos[i].ParameterType.Name} - DefaultValue:'{optionalParamsInfos[i].RawDefaultValue}'";
                    }
                }

                Write(SpectreWritingType.Warning, missingOptionalMessage);
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
                        var value = ArgPreprocessor.CoerceArgument(typeRequired, command.Arguments.ElementAt(i));
                        methodParameterValueList.RemoveAt(i);
                        methodParameterValueList.Insert(i, value);
                    }
                    // ToDo: Handle Errors
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

            object[] inputArgs = null;
            if (methodParameterValueList.Count > 0)
            {
                inputArgs = methodParameterValueList.ToArray();
            }

            // Command Type
            var typeInfo = methodDictionary.Item3;

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
    }
}