using System;
using System.Linq;
using System.Reflection;
using System.Data.Common;
using System.Collections.Generic;

using KarmaCounterServer.Model;
using KarmaCounterServer.ModelMapping.AttributeTemplates;

namespace KarmaCounterServer.ModelMapping
{
    public class ModelMapper
    {
        public DbMappingInfo MapFromModel<M, TB, FN>(M mask = null)
            where M : class, IModelElement, new()
            where TB : DbMappingTableAttribute
            where FN : DbMappingAttribute => MapFromModel<M, TB, FN, ForeignIgnore, SelectIgnore>(mask);

        public DbMappingInfo MapFromModel<M, TB, FN, FK>(M mask = null)
            where M : class, IModelElement, new()
            where TB : DbMappingTableAttribute
            where FN : DbMappingAttribute
            where FK : DbMappingForeignAttribute => MapFromModel<M, TB, FN, FK, SelectIgnore>(mask);

        public DbMappingInfo MapFromModel<M, TB, FN, FK, WC>(M mask = null)
            where M : class, IModelElement, new()
            where TB : DbMappingTableAttribute
            where FN : DbMappingAttribute
            where FK : DbMappingForeignAttribute
            where WC : DbMappingAttribute =>
            MapFromModel<TB, FN, FK, WC>(typeof(M), mask);

        public M MapToModelSingle<M, TB, FN>(DbDataReader reader)
            where M : class, IModelElement, new()
            where TB : DbMappingTableAttribute
            where FN : DbMappingAttribute =>
            MapToModelSingle<TB, FN, ForeignIgnore>(reader, true, typeof(M)) as M;

        public M MapToModelSingle<M, TB, FN, FK>(DbDataReader reader) 
            where M : class, IModelElement, new()
            where TB : DbMappingTableAttribute
            where FN : DbMappingAttribute
            where FK : DbMappingForeignAttribute =>
            MapToModelSingle<TB, FN, FK>(reader, true, typeof(M)) as M;

        public List<M> MapToModel<M, TB, FN>(DbDataReader reader)
            where M : class, IModelElement, new()
            where TB : DbMappingTableAttribute
            where FN : DbMappingAttribute => MapToModel<M, TB, FN, ForeignIgnore>(reader);

        public List<M> MapToModel<M, TB, FN, FK>(DbDataReader reader)
            where M : class, IModelElement, new()
            where TB : DbMappingTableAttribute
            where FN : DbMappingAttribute
            where FK : DbMappingForeignAttribute
        {
            List<M> result = new List<M>();

            M item = MapToModelSingle<M, TB, FN, FK>(reader);
            while (item != null)
            {
                result.Add(item);
                item = MapToModelSingle<M, TB, FN, FK>(reader);
            }

            return result;
        }

        private object MapToModelSingle<TB, FN, FK>(DbDataReader reader, bool move, Type T = null) 
            where TB : DbMappingTableAttribute
            where FN : DbMappingAttribute
            where FK : DbMappingForeignAttribute
        {
            if (!reader.HasRows || (move && !reader.Read()))
                return null;

            object model = T.GetConstructor(new Type[] { }).Invoke(new object[] { });

            List<PropertyInfo> modelProps = T.GetProperties().ToList().Concat(T.GetRuntimeProperties()).Distinct().ToList();

            List<PropertyInfo> mappedModelProps = modelProps.Where(P => P.GetCustomAttribute<FN>() != null).ToList();
            List<PropertyInfo> foreignProperties = modelProps.Where(P => P.GetCustomAttribute<FK>() != null).ToList();

            foreach (PropertyInfo prop in mappedModelProps)
            {
                object val = reader[prop.GetCustomAttribute<FN>().Alias];

                if (DBNull.Value.Equals(val))
                    continue;

                prop.SetValue(model, val);
            }

            foreach(PropertyInfo foreignProp in foreignProperties)
                foreignProp.SetValue(model, MapToModelSingle<TB, FN, FK>(reader, false, foreignProp.PropertyType));

            return model;
        }

        private DbMappingInfo MapFromModel<TB, FN, FK, WC>(Type T, IModelElement mask = null) 
            where TB : DbMappingTableAttribute
            where FN : DbMappingAttribute 
            where FK : DbMappingForeignAttribute
            where WC : DbMappingAttribute
        {
            TB tableAttribute = T.GetCustomAttribute<TB>();
            string tableName = tableAttribute.TableName;
            string primaryKey = tableAttribute.PrimaryKey;

            List<PropertyInfo> modelProps = T.GetProperties().ToList().Concat(T.GetRuntimeProperties()).Distinct().ToList();

            List<PropertyInfo> mappedModelProps = modelProps.Where(P => P.GetCustomAttribute<FN>() != null).ToList();
            List<PropertyInfo> foreignProperties = modelProps.Where(P => P.GetCustomAttribute<FK>() != null).ToList();
            List<PropertyInfo> whereConstProperties = modelProps.Where(P => P.GetCustomAttribute<WC>() != null).ToList();

            List<(string, string, object)> fields = mappedModelProps.Aggregate(
                new List<(string, string, object)>(),
                (A, P) =>
                {
                    FN attr = P.GetCustomAttribute<FN>();

                    if (typeof(IModelElement).IsAssignableFrom(P.PropertyType))
                        return A.Append(MapFromModel<TB, FN, FK, WC>(P.PropertyType, P.GetValue(mask) as IModelElement).KeysValues.
                                                                                 Select(KV => (attr.Name, KV.alias, KV.value)).First()).ToList();

                    return A.Append((attr.Name, attr.Alias, attr.MapValue ? (attr.DefaultValue ?? (mask == null ? null : P.GetValue(mask))) : null)).ToList();
                }).ToList();

            List<(string, object)> whereConsts = whereConstProperties.Aggregate(
                new List<(string, object)>(),
                (A, P) =>
                {
                    WC attr = P.GetCustomAttribute<WC>();
                    object val = attr.DefaultValue == null ? (mask == null ? null : P.GetValue(mask)) : attr.DefaultValue;
                    return A.Append((attr.Name, val)).ToList();
                }
            );

            List<(string innerKeyName, string outerKeyName, DbMappingInfo info)> foreign = foreignProperties.Select(
                FP => (
                    FP.GetCustomAttribute<FK>().InnerName, 
                    FP.GetCustomAttribute<FK>().OuterName,
                    MapFromModel<TB, FN, FK, WC>(FP.PropertyType, (mask == null ? null : 
                        (FP.GetValue(mask) ?? FP.PropertyType.GetConstructor(new Type[] { }).Invoke(new object[] { }))) as IModelElement)
                )
            ).ToList();

            return new DbMappingInfo(tableName, primaryKey, whereConsts, fields, foreign);
        }
    }
}