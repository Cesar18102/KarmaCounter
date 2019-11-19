using System.Data.Common;

using MySql.Data.MySqlClient;

namespace DbUtil
{
    public class MySqlRepoFactory : RepoFactory
    {
        public MySqlRepoFactory(string server, string database, string user, string password) : 
            base(server, database, user, password) { }

        private string ConnectionString => 
            $"Server={Server}; Database={Database}; Uid={User}; Pwd={Password};";

        public override DbConnection GetConnection() => 
            new MySqlConnection(ConnectionString);

        public override DbParameter CreateParameter(string name, object value) => 
            new MySqlParameter(name, value);
    }
}