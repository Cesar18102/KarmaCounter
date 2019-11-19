using DbUtil;

namespace Logger.Util
{
    public class DbLogConfig
    {
        public string LogTableName { get; private set; }
        public string LogCaptionFieldName { get; private set; }
        public string LogDateTimeFieldName { get; private set; }
        public string LogMessageFieldName { get; private set; }

        public DbLogConfig(string logTableName, string logCaptionFieldName, string logDateTimeFieldName, string logMessageFieldName)
        {
            LogTableName = logTableName;
            LogCaptionFieldName = logCaptionFieldName;
            LogDateTimeFieldName = logDateTimeFieldName;
            LogMessageFieldName = logMessageFieldName;
        }
    }
}
