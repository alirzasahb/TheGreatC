using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using TheGreatC.Common;

namespace TheGreatC.Domain
{
    public static class Commander
    {

        public static readonly List<Tuple<string, Dictionary<string, IEnumerable<ParameterInfo>>, Type>> CommandLibraries = new();

        public static void LoadCommands()
        {
            // Use reflection to load all of the classes in the Commands namespace:

            var commandsAssembly =
                GetCommandsLibAssemblyByName(ConfigurationManager.SharedConfigurations["Settings:CommandsNamespace"]);

            // Get Classes And Make Check Constructors Not Included In CommandClasses
            var commandClasses = commandsAssembly.GetTypes()
                .Where(assembly => assembly.IsClass && !assembly.Name.Contains("<>c")).ToList();

            foreach (var commandClass in commandClasses)
            {
                // Load the method info from each class into a dictionary:
                var methods = commandClass.GetMethods(BindingFlags.Static | BindingFlags.Public);
                var methodDictionary = new Dictionary<string, IEnumerable<ParameterInfo>>();
                foreach (var method in methods)
                {
                    var commandName = method.Name;
                    methodDictionary.Add(commandName, method.GetParameters());
                }

                // Add the dictionary of methods for the current class into a dictionary of command classes:
                CommandLibraries.Add(
                    new Tuple<string, Dictionary<string, IEnumerable<ParameterInfo>>, Type>(commandClass.Name,
                        methodDictionary, commandClass));
            }
        }

        private static Assembly GetCommandsLibAssemblyByName(string assemblyName)
        {
            try
            {
                // ToDo: Create And Support Multiple AppDomain For Multiple DLLs
                var path = Path.GetDirectoryName(AppDomain.CurrentDomain.BaseDirectory);
                var assemblies = Directory.GetFiles(path ?? throw new InvalidOperationException(), "*.dll")
                    .Select(Assembly.LoadFile).ToList();
                return assemblies.FirstOrDefault(a => a.GetName().Name == assemblyName);
            }
            // ToDo: Handle Errors
            catch (Exception)
            {
                throw;
            }
        }
    }
}