using System.Linq;
using System.Collections.Generic;

namespace KarmaCounterServer.ModelMapping
{
    public class DbMappingInfo
    {
        public string TableName { get; private set; }
        public string PrimaryKey { get; private set; }
        public List<(string key, object value)> Wheres { get; private set; }
        public List<(string key, string alias, object value)> Fields { get; private set; }
        public List<(string innerKeyName, string outerKeyName, DbMappingInfo info)> Foreign { get; private set; }

        public DbMappingInfo(string tableName, string primaryKey,
                             List<(string key, object value)> wheres = null,
                             List<(string key, string alias, object value)> fields = null,
                             List<(string innerKeyName, string outerKeyName, DbMappingInfo info)> foreign = null)
        {
            TableName = tableName;
            PrimaryKey = primaryKey;
            Wheres = wheres == null ? new List<(string key, object value)>() : wheres;
            Fields = fields == null ? new List<(string key, string alias, object value)>() : fields;
            Foreign = foreign == null ? new List<(string innerKeyName, string outerKeyName, DbMappingInfo info)>() : foreign;
        }

        public List<(string t, string key, object value)> WhereConsts =>
            Wheres.Select(W => (TableName, W.key, W.value)).ToList();

        public List<(string t, string key, object value)> AllWhereConsts =>
            WhereConsts.Concat(Foreign.Aggregate(new List<(string t, string key, object value)>(), (A, F) => A.Concat(F.info.AllWhereConsts).ToList())).ToList();

        public List<(string t, string key, string alias, object value)> KeysValues =>
            Fields.Select(F => (TableName, F.key, F.alias, F.value)).ToList();

        public List<(string t, string key, string alias, object value)> AllKeyValues =>
            KeysValues.Concat(Foreign.Aggregate(new List<(string t, string key, string alias, object value)>(), 
                                                (A, F) => A.Concat(F.info.AllKeyValues).ToList())
            ).ToList();

        public List<(string t1, string key1, string t2, string key2)> Equations =>
            Foreign.Select(F => (TableName, F.innerKeyName, F.info.TableName, F.outerKeyName)).ToList();

        public List<(string t1, string key1, string t2, string key2)> AllEquations =>
            Equations.Concat(
                Foreign.Select(F => F.info.Equations).
                Aggregate(new List<(string t1, string key1, string t2, string key2)>(), (A, F) => A.Concat(F).ToList())
            ).ToList();

        public List<string> AllTables => new List<string>() { TableName }.
            Concat(Foreign.Aggregate(new List<string>(), (A, F) => A.Concat(F.info.AllTables).ToList())).ToList();
    }
}