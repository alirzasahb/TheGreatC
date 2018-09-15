using System;
using TheGreatC.Common;
using TheGreatC.Runtime;

namespace TheGreatC
{
    internal static class Startup
    {
        // Startup Pipeline
        public static void Run()
        {
            SharedConfigurations.Build();
            Console.Title = ConfigurationManager.SharedConfigurations["Title"];
            Core.Instance.Start();
        }

        // Call SharedConfigurations.Build Once To Build It
        private static class SharedConfigurations
        {
            public static void Build()
            {
                ConfigurationManager.Build();
            }
        }
    }
}