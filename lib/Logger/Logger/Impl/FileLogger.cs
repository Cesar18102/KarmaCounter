using System;
using System.IO;
using System.Threading.Tasks;

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

        public async Task Trace(string log) => await Trace("TRACE", log);
        public async Task Error(Exception ex) => await Trace("ERROR", $"{ex.GetType().Name}: {ex.Message}");

        public async Task Trace(string caption, string log)
        {
            using (StreamWriter LogStream = new StreamWriter(LogFileName, true))
            {
                DateTime now = DateTime.Now;
                await LogStream.WriteLineAsync($"{now.ToShortDateString()} {now.ToShortTimeString()} {caption}: {log}");
            }
        }
    }
}