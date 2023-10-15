using System;
using System.Text;
using TheGreatC.Common;
using TheGreatC.Common.Utilities;
using TheGreatC.Runtime;

namespace TheGreatC
{
    internal static class Program
    {
        private static void Main()
        {
            Console.OutputEncoding = Encoding.UTF8;
            Console.InputEncoding = Encoding.UTF8;
            ConfigurationManager.Build();
            Console.Title = ConfigurationManager.SharedConfigurations["Title"];
            SpectreConsoleWriter.WriteVersion();
            // ToDo: Full Screen Launch
            Core.Start();
        }
    }
}