using System;
using System.Configuration;

namespace TheGreatC.Common.Configuration
{
    public class Properties
    {
        public static readonly string CommandsNamespace;
        public static readonly string LogPath;
        public static readonly bool LogFeature;


        static Properties()
        {
            CommandsNamespace = ConfigurationManager.AppSettings["CommandsNameSpace"];
            LogPath = ConfigurationManager.AppSettings["LogPath"];
            LogFeature = Convert.ToBoolean(ConfigurationManager.AppSettings["LogFeature"]);
        }
    }
}
