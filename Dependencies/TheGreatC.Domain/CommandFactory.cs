using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Collections.Generic;
using TheGreatC.Common.Configuration;


namespace TheGreatC.Domain
{
    public class CommandFactory
    {
        protected static readonly string CommandNameSpace = Properties.CommandsNamespace;
        protected static Dictionary<string, Dictionary<string, IEnumerable<ParameterInfo>>> CommandLibraries;

        protected static void LoadInstalledCommands()
        {
            // console are located in the commands namespace. Load 
            // references to each type in that namespace via reflection:
            CommandLibraries = new Dictionary<string, Dictionary<string,
                IEnumerable<ParameterInfo>>>();

            // Use reflection to load all of the classes in the Commands namespace:

            #region When Commands Embedded In Current Project

            //var q = from t in Assembly.GetExecutingAssembly().GetTypes()
            //    where t.IsClass && t.Namespace == _commandNameSpace
            //    select t;
            //var commandClasses = q.ToList();

            #endregion

            #region When Commands Embedded In External Project

            var commandsAssembly = GetAssemblyByName(CommandNameSpace);
            //var commandClasses = commandsAssembly.GetTypes().Where(t => t.IsClass).ToList();
            // Get Classes And Make Check Constructors Not Included In CommandClasses
            var commandClasses = commandsAssembly.GetTypes().Where(assembly => assembly.IsClass && !assembly.Name.Contains("<>c")).ToList();

            #endregion


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
                CommandLibraries.Add(commandClass.Name, methodDictionary);
            }
        }

        public static Assembly GetAssemblyByName(string assemblyName)
        {
            try
            {
                // ToDo: Create And Support Multiple AppDomain For Multiple DLLs
                var path = Path.GetDirectoryName(AppDomain.CurrentDomain.BaseDirectory);
                var assemblies = Directory.GetFiles(path ?? throw new InvalidOperationException(), "*.dll").Select(Assembly.LoadFile).ToList();
                return assemblies.FirstOrDefault(a => a.GetName().Name == assemblyName);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
