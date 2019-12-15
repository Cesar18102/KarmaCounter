using System;
using System.Data.Common;
using System.Threading.Tasks;
using System.Collections.Generic;

using DbUtil;

using Autofac;

using KarmaCounterServer.Model;
using KarmaCounterServer.ModelMapping;
using KarmaCounterServer.ModelMapping.AttributeTemplates;
using KarmaCounterServer.ModelMapping.AttributeTemplates.Ownership;

namespace KarmaCounterServer.DataAccess
{
    public class OwnershipDataAccess : DataAccess<Ownership>
    {
        public override Task<Ownership> Create(Ownership model) =>
            throw new NotSupportedException();

        public async Task<Ownership> GetByGroupId(long id)
        {
            IRepoFactory repoFactory = Global.DI.Resolve<IRepoFactory>();
            ModelMapper mapper = Global.DI.Resolve<ModelMapper>();

            using (DbConnection connection = repoFactory.GetConnection())
            {
                DbMappingInfo groupSelectInfo = mapper.MapFromModel<Group, TableAttribute, OwnershipSelect, OwnershipSelectForeign, OwnershipSelectWhereGroup>(new Group(id));

                (string cmdText, List<(string key, object val)> par) cmdSelectInfo = groupSelectInfo.CreateSelectText();
                DbCommand cmdSelect = CreateCommand(cmdSelectInfo.cmdText, connection, repoFactory, cmdSelectInfo.par);

                await connection.OpenAsync();

                using (DbDataReader reader = await cmdSelect.ExecuteReaderAsync())
                    return mapper.MapToModelSingle<Ownership, TableAttribute, OwnershipSelect, OwnershipSelectForeign>(reader);
            }
        }

        public async Task<List<Ownership>> GetByOwnerUserId(long id)
        {
            IRepoFactory repoFactory = Global.DI.Resolve<IRepoFactory>();
            ModelMapper mapper = Global.DI.Resolve<ModelMapper>();

            using (DbConnection connection = repoFactory.GetConnection())
            {
                DbMappingInfo ownershipSelectInfo = mapper.MapFromModel<Ownership, TableAttribute, OwnershipSelect, OwnershipSelectForeign, OwnershipSelectWhereUser>(new Ownership(new User(id)));

                (string cmdText, List<(string key, object val)> par) cmdSelectInfo = ownershipSelectInfo.CreateSelectText();
                DbCommand cmdSelect = CreateCommand(cmdSelectInfo.cmdText, connection, repoFactory, cmdSelectInfo.par);

                await connection.OpenAsync();

                using (DbDataReader reader = await cmdSelect.ExecuteReaderAsync())
                    return mapper.MapToModel<Ownership, TableAttribute, OwnershipSelect, OwnershipSelectForeign>(reader);
            }
        }

        public override Task<Ownership> Delete(Ownership model)
        {
            throw new NotImplementedException();
        }

        public override Task<Ownership> Update(Ownership model)
        {
            throw new NotImplementedException();
        }
    }
}