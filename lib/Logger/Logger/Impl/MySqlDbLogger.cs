using System;
using System.Data.Common;

using DbUtil;

using Logger.Util;

namespace Logger.Impl
{
    public class MySqlDbLogger : DbLogger
    {
        public MySqlDbLogger(MySqlRepoFactory repoFactory, DbLogConfig logConfig) : base(repoFactory, logConfig) { }

        protected override string GetLogInsertCmdText() =>
            $"INSERT INTO {LogConfig.LogTableName} " +
            $"VALUES(0, {LOG_CAPTION_PARAM_NAME}, {LOG_DATE_TIME_PARAM_NAME}, {LOG_MESSAGE_PARAM_NAME})";
    }
}
