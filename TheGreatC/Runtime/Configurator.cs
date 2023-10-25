using System;
using System.Text;
using TheGreatC.Common;
using TheGreatC.Common.Internal.Utilities;

namespace TheGreatC.Runtime
{
    public static class Configurator
    {
        public static void StartUp()
        {
            Console.OutputEncoding = Encoding.UTF8;
            Console.InputEncoding = Encoding.UTF8;
            ConfigurationManager.Build();
            Console.Title = ConfigurationManager.SharedConfigurations["Settings:Title"];
            if (bool.Parse(ConfigurationManager.SharedConfigurations["Settings:ShowSplash"]))
                SpectreConsoleWriter.WriteVersion();
        }
    }
}
