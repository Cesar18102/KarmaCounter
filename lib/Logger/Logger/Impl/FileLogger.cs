using System;
using System.IO;

namespace Logger
{
    public class FileLogger : ILogger
    {
        public string LogFileName { get; private set; }

        public FileLogger(string logDirName)
        {
            if (!Directory.Exists(logDirName))
                Directory.CreateDirectory(logDirName);

            DateTime now = DateTime.Now;
            LogFileName = logDirName + "/Log_" + (now.ToShortDateString() + "_" + now.ToLongTimeString() + ".txt").Replace(":", ".");

            using (File.CreateText(LogFileName)) { }
        }

        public void Debug(string log) => Log("DEBUG", log);
        public void Error(Exception ex) => Log("ERROR", $"{ex.GetType().Name}: {ex.Message}");

        public void Log(string caption, string log)
        {
            using (StreamWriter LogStream = new StreamWriter(LogFileName, true))
            {
                DateTime now = DateTime.Now;
                LogStream.WriteLine($"{now.ToShortDateString()} {now.ToShortTimeString()} {caption}: {log}");
            }
        }
    }
}