using System;
using System.Data.Common;
using System.Threading.Tasks;

using DbUtil;

using Logger.Util;

namespace Logger.Impl
{
    public abstract class DbLogger : ILogger
    {
        protected const string LOG_CAPTION_PARAM_NAME = "@caption";
        protected const string LOG_DATE_TIME_PARAM_NAME = "@datetime";
        protected const string LOG_MESSAGE_PARAM_NAME = "@message";

        public IRepoFactory RepoFactory { get; private set; }
        public DbLogConfig LogConfig { get; private set; }

        public DbLogger(IRepoFactory repoFactory, DbLogConfig logConfig)
        {
            RepoFactory = repoFactory;
            LogConfig = logConfig;
        }

        public async Task Trace(string log) => await Trace("TRACE", log);
        public async Task Error(Exception ex) => await Trace("ERROR", $"{ex.GetType().Name}: {ex.Message}");

        public async Task Trace(string caption, string log)
        {
            using (DbConnection connection = RepoFactory.GetConnection())
            {
                DbCommand cmd = RepoFactory.CreateCommand(GetLogInsertCmdText(), connection);

                cmd.Parameters.Add(RepoFactory.CreateParameter(LOG_CAPTION_PARAM_NAME, caption));
                cmd.Parameters.Add(RepoFactory.CreateParameter(LOG_DATE_TIME_PARAM_NAME, DateTime.Now));
                cmd.Parameters.Add(RepoFactory.CreateParameter(LOG_MESSAGE_PARAM_NAME, log));

                await connection.OpenAsync();
                await cmd.ExecuteNonQueryAsync();
            }
        }

        /// <summary>
        /// Should build log insert command text using basic constant param names and LogConfig property
        /// </summary>
        /// <returns></returns>
        protected abstract string GetLogInsertCmdText();
    }
}
