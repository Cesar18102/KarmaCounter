using System;
using System.Linq;
using System.Data.Common;

using DbUtil;

using System.Collections.Generic;

namespace KarmaCounterServer.ModelMapping
{
    public static class MappingToQueryConverter
    {
        public static (string, List<(string param, object value)>) CreateSelectText(this DbMappingInfo mapping)
        {
            List<(string table, string key, string param, object value)> parameters =
                mapping.AllKeyValues.Where(K => K.value != null).
                Select(K => (K.t, K.key, K.value)).
                Concat(mapping.AllWhereConsts).
                Select(K => (K.t, K.key, $"@{K.key}", K.value)).ToList();

            List<string> whereConstraints = mapping.AllEquations.Select(EQ => $"{EQ.t1}.{EQ.key1} = {EQ.t2}.{EQ.key2}").
                                                                 Concat(parameters.Select(K => $"{K.table}.{K.key} = @{K.key}")).ToList();

            string cmdText = $"SELECT {String.Join(", ", mapping.AllKeyValues.Select(K => $"{K.t}.{K.key} AS {K.alias}"))} " +
                             $"FROM {String.Join(", ", mapping.AllTables)}" +
                             (whereConstraints.Count == 0 ? ";" :
                             $" WHERE {String.Join(" AND ", whereConstraints)};");

            return (cmdText, parameters.Select(P => (P.param, P.value)).ToList());
        }

        public static (string, List<(string param, object value)>) CreateInsertText(this DbMappingInfo mapping, string tempTable = "")
        {
            string insertedTempTable = $"@{String.Join("_", mapping.Foreign.Select(F => F.info.TableName).Prepend(mapping.TableName))}";

            List<(string innerKey, string outerKey, (string cmdText, List<(string param, object value)> parameters) cmd)> foreign = 
                mapping.Foreign.Select(F => (F.innerKeyName, F.outerKeyName, F.info.CreateInsertText(insertedTempTable))).ToList();

            List <(string key, string param, object value)> parameters = 
                mapping.KeysValues.Where(K => K.key != mapping.PrimaryKey).Select(K => (K.key, $"@{K.key}", K.value)).ToList();

            string cmdText =
                (foreign.Count == 0 ? "" : $"DECLARE {insertedTempTable} TABLE({String.Join(", ", foreign.Select(F => F.outerKey).Select(F => F + " INT"))});") +
                string.Join(" ", foreign.Select(F => F.cmd.cmdText)) +
                $"INSERT INTO {mapping.TableName} ({String.Join(", ", parameters.Select(P => P.key).Concat(mapping.Foreign.Select(F => F.innerKeyName)))}) " +
                (tempTable == "" ? $"OUTPUT INSERTED.{mapping.PrimaryKey} " : $"OUTPUT {String.Join(", ", mapping.Foreign.Select(F => F.innerKeyName).Prepend(mapping.PrimaryKey).Select(K => $"INSERTED.{K}"))} INTO {tempTable} ") +
                $"VALUES({String.Join(", ", parameters.Select(P => P.param).Concat(foreign.Select(F => $"(SELECT TOP 1 {F.outerKey} FROM {insertedTempTable})")))});";

            return (cmdText, parameters.Select(P => (P.param, P.value)).
                                        Concat(foreign.Aggregate(new List<(string, object)>(), (A, F) => A.Concat(F.cmd.parameters).ToList())).ToList());
        }

        public static (string, List<(string param, object value)>) CreateUpdateText(this DbMappingInfo mapping)
        {
            List<(string table, string key, string param, object value)> parameters =
                mapping.KeysValues.Where(K => K.value != null && K.key != mapping.PrimaryKey).
                Select(K => (K.t, K.key, $"@{K.key}", K.value)).ToList();

            List<string> whereConstraints = mapping.AllEquations.Select(EQ => $"{EQ.t1}.{EQ.key1} = {EQ.t2}.{EQ.key2}").
                                                                 Concat(mapping.WhereConsts.Select(K => $"{K.key} = @{K.key}")).ToList();

            string cmdText = $"UPDATE {mapping.TableName} " +
                             $"SET {String.Join(", ", parameters.Select(K => $"{K.key} = @{K.key}"))}" +
                             (whereConstraints.Count == 0 ? ";" : $" WHERE {String.Join(" AND ", whereConstraints)};");

            return (cmdText, parameters.Select(P => (P.param, P.value)).Concat(mapping.WhereConsts.Select(W => ($"@{W.key}", W.value))).ToList());
        }
    }
}