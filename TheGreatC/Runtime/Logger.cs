using System;
using System.IO;
using TheGreatC.Common;

namespace TheGreatC.Runtime
{
    public static class Logger
    {
        public static void Log(string message)
        {
            try
            {
                var todayFile = DateTime.Now.Date.Day + "." + DateTime.Now.Date.Month + "." + DateTime.Now.Date.Year +
                                ".txt";
                var mainfolder = ConfigurationManager.SharedConfigurations["LogPath"];
                if (!Directory.Exists(mainfolder))
                {
                    Directory.CreateDirectory(mainfolder);
                }

                var logFolderPathString = Path.Combine(mainfolder, "The Great C - Log");
                if (!Directory.Exists(logFolderPathString))
                {
                    Directory.CreateDirectory(logFolderPathString);
                }

                var yearLogFolderPathString = Path.Combine(logFolderPathString, DateTime.Now.ToString("yyyy"));
                if (!Directory.Exists(yearLogFolderPathString))
                {
                    Directory.CreateDirectory(yearLogFolderPathString);
                }

                var monthLogFolderPathString = Path.Combine(yearLogFolderPathString, DateTime.Now.ToString("MM"));
                if (!Directory.Exists(monthLogFolderPathString))
                {
                    Directory.CreateDirectory(monthLogFolderPathString);
                }

                logFolderPathString = Path.Combine(monthLogFolderPathString, todayFile);

                var fs = new FileStream(logFolderPathString, FileMode.OpenOrCreate, FileAccess.Write);
                var sw = new StreamWriter(fs);
                sw.BaseStream.Seek(0, SeekOrigin.End);
                sw.WriteLine(" ---------------------------------------- ");
                sw.WriteLine(" --!-- Error Message --!-- ");
                sw.WriteLine($" ---- {DateTime.UtcNow} ---- ");
                sw.WriteLine(message);
                sw.WriteLine(" ---------------------------------------- ");
                sw.Flush();
                sw.Close();
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}