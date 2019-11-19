using System.Data.Common;

namespace DbUtil
{
    public abstract class RepoFactory : IRepoFactory
    {
        public string Server { get; private set; }
        public string Database { get; private set; }
        public string User { get; private set; }
        protected string Password { get; set; }

        public RepoFactory(string server, string database, string user, string password)
        {
            Server = server;
            Database = database;
            User = user;
            Password = password;
        }

        public DbCommand CreateCommand(string cmdText, DbConnection connection)
        {
            DbCommand command = connection.CreateCommand();
            command.CommandText = cmdText;
            return command;
        }

        public abstract DbParameter CreateParameter(string name, object value);
        public abstract DbConnection GetConnection();
    }
}
