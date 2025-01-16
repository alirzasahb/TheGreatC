using System;
using System.Collections.Generic;
using System.Globalization;
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
            if (Commander.CommandLibraries.TrueForAll(x => !string.Equals(x.Item1, command.LibraryClassName, StringComparison.Ordinal)))
            {
                return new CommandResult()
                {
                    Result = null,
                    Response = new CommandResponse()
                    {
                        Message = $"Command Library: {command.LibraryClassName} Was Not Found!",
                        IsSuccessful = false,
                        IsNotFound = true,
                    },
                };
            }

            var methodDictionary =
                Commander.CommandLibraries.First(x => string.Equals(x.Item1, command.LibraryClassName, StringComparison.OrdinalIgnoreCase));

            if (!methodDictionary.Item2.ContainsKey(command.Name))
            {
                // Check For Similar Commands From Same Library To Offer
                var similarCommands = methodDictionary.Item2
                    .Where(c => c.Key.ToLower(CultureInfo.InvariantCulture).Contains(command.Name.ToLower(CultureInfo.InvariantCulture), StringComparison.Ordinal))
                    .Take(5).ToList();

                var similarCommandsMessage = new List<string> { "Did You Mean: " };
                if (similarCommands.Any())
                    similarCommandsMessage.AddRange(similarCommands.Select(similarCommand => $"•   {similarCommand.Key}"));

                return new CommandResult()
                {
                    Result = similarCommandsMessage.Count >= 2 ? similarCommandsMessage : null,
                    Response = new CommandResponse()
                    {
                        Message = $"Command: {command.Name} Was Not Found!",
                        IsSuccessful = false,
                        IsNotFound = true,
                    },
                };
            }

            // Make sure the corret number of required arguments are provided:
            // -------------------------------------------------------------------------------

            var methodParameterValueList = new List<object>();
            IEnumerable<ParameterInfo> paramInfoList = methodDictionary.Item2[command.Name].ToList();

            // Validate proper # of required arguments provided. Some may be optional:
            var requiredParams = paramInfoList.Where(p => !p.IsOptional);
            var optionalParams = paramInfoList.Where(p => p.IsOptional);
            var requiredParamsInfos = requiredParams as ParameterInfo[] ?? requiredParams.ToArray();
            var optionalParamsInfos = optionalParams as ParameterInfo[] ?? optionalParams.ToArray();
            var commandArgs = new CommandArgumentDetail()
            {
                OptionalArgs = optionalParamsInfos.Length,
                ProvidedArgs = command.Arguments.Count(),
                RequiredArgs = requiredParamsInfos.Length,
            };

            if (commandArgs.RequiredArgs > commandArgs.ProvidedArgs)
            {
                var missingRequiredArgs = requiredParamsInfos.Select(x => x.Name).ToList();

                var missingRequiredArgsMessage = commandArgs.RequiredArgs >= 2
                    ? string.Create(CultureInfo.InvariantCulture, $"Error: 'Missing Required Arguments For Command Found' - {commandArgs.RequiredArgs} Required - Arguments:")
                    : string.Create(CultureInfo.InvariantCulture, $"Error: 'Missing Required Argument For Command Found' - {commandArgs.RequiredArgs} Required - Argument:");

                for (var i = 0; i < missingRequiredArgs.Count; i++)
                {
                    missingRequiredArgsMessage += $" {missingRequiredArgs[i]}:{requiredParamsInfos[i].ParameterType.Name},";
                }

                return new CommandResult()
                {
                    Result = null,
                    Response = new CommandResponse()
                    {
                        Message = missingRequiredArgsMessage.Remove(missingRequiredArgsMessage.Length - 1),
                        IsSuccessful = false,
                    },
                };
            }

            // Warn User About Optional Argument(s)
            if (commandArgs.OptionalArgs + commandArgs.RequiredArgs > commandArgs.ProvidedArgs)
            {
                var missingOptionalArgs = optionalParamsInfos.Select(x => x.Name).ToList();
                var missingOptionalArgsMessage = commandArgs.OptionalArgs >= 2
                    ? string.Create(CultureInfo.InvariantCulture, $"Warning: 'Missing Optional Arguments For Following Command' - {commandArgs.OptionalArgs} Optional - Arguments:")
                    : string.Create(CultureInfo.InvariantCulture, $"Warning: 'Missing Optional Argument For Following Command' - {commandArgs.OptionalArgs} Optional - Argument:");

                for (var i = 0; i < missingOptionalArgs.Count; i++)
                {
                    missingOptionalArgsMessage +=
                        $" {missingOptionalArgs[i]}:{optionalParamsInfos[i].ParameterType.Name} - DefaultValue:'{optionalParamsInfos[i].RawDefaultValue}',";
                }

                Write(SpectreWritingType.Warning, missingOptionalArgsMessage.Remove(missingOptionalArgsMessage.Length - 1));
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
                        throw new ArgumentException(message, argumentName);
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
                    binder: null, target: null, inputArgs, CultureInfo.InvariantCulture);

                return new CommandResult()
                {
                    Result = result,
                    Response = new CommandResponse()
                    {
                        Message = "",
                        IsSuccessful = true,
                    },
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