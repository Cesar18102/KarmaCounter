using System.Data.Common;
using System.Threading.Tasks;
using System.Collections.Generic;

using DbUtil;

using KarmaCounterServer.Model;

namespace KarmaCounterServer.DataAccess
{
    public abstract class DataAccess<T> : IDataAccess<T> where T : IModelElement
    {
        public abstract Task<T> Create(T model);
        public abstract Task<T> Delete(T model);
        public abstract Task<T> Update(T model);

        public DbCommand CreateCommand(string cmdText, DbConnection connection, IRepoFactory repoFactory, List<(string, object)> parameters)
        {
            DbCommand cmd = repoFactory.CreateCommand(cmdText, connection);

            foreach ((string key, object val) in parameters)
                cmd.Parameters.Add(repoFactory.CreateParameter(key, val));

            return cmd;
        }
    }
}