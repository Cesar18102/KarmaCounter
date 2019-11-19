using System.Data.Common;

namespace DbUtil
{
    public interface IRepoFactory
    {
        DbConnection GetConnection();
        DbCommand CreateCommand(string cmdText, DbConnection connection);
        DbParameter CreateParameter(string name, object value);
    }
} 