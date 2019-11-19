using System;
using System.Data.Common;
using System.Data.SqlClient;

namespace DbUtil
{
    public class MsSqlRepoFactory : RepoFactory
    {
        public int PacketSize { get; private set; }
        public bool PersistSecurity { get; private set; }

        public MsSqlRepoFactory(string server, string database, string user, string password,
                                int packetSize = 4096, bool persistSecurity = false) : base(server, database, user, password)
        {
            PacketSize = packetSize;
            PersistSecurity = persistSecurity;
        }

        private string ConnectionString =>
            $"workstation id={Server}; packet size={PacketSize}; user id={User}; pwd={Password}; " +
            $"data source={Server}; persist security info={(PersistSecurity ? "True" : "False")}; initial catalog={Database}";

        public override DbParameter CreateParameter(string name, object value) => 
            new SqlParameter(name, value);

        public override DbConnection GetConnection() =>
            new SqlConnection(ConnectionString);
    }
}
