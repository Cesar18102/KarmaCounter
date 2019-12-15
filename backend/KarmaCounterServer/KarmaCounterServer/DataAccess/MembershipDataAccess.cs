using System;
using System.Data.Common;
using System.Threading.Tasks;
using System.Collections.Generic;

using Autofac;

using DbUtil;

using KarmaCounterServer.Model;
using KarmaCounterServer.ModelMapping;
using KarmaCounterServer.ModelMapping.AttributeTemplates;
using KarmaCounterServer.ModelMapping.AttributeTemplates.Membership;

namespace KarmaCounterServer.DataAccess
{
    public class MembershipDataAccess : DataAccess<Membership>
    {
        public override async Task<Membership> Create(Membership model)
        {
            IRepoFactory repoFactory = Global.DI.Resolve<IRepoFactory>();
            ModelMapper mapper = Global.DI.Resolve<ModelMapper>();
            Membership newMembership = null;

            using (DbConnection connection = repoFactory.GetConnection())
            {
                DbMappingInfo membershipInsertInfo = mapper.MapFromModel<Membership, TableAttribute, MembershipInsert, ForeignIgnore, MembershipInsert>(model);

                (string cmdText, List<(string key, object val)> par) cmdInsertInfo = membershipInsertInfo.CreateInsertText();
                DbCommand cmd = CreateCommand(cmdInsertInfo.cmdText, connection, repoFactory, cmdInsertInfo.par);

                await connection.OpenAsync();

                using (DbDataReader reader = await cmd.ExecuteReaderAsync())
                    newMembership = mapper.MapToModelSingle<Membership, TableAttribute, MembershipSelectInserted, ForeignIgnore>(reader);
            }

            return await GetById(newMembership.Id);
        }

        public async Task<Membership> GetById(long id)
        {
            //TODO
            return null;
        }

        public override Task<Membership> Delete(Membership model)
        {
            throw new NotImplementedException();
        }

        public override Task<Membership> Update(Membership model)
        {
            throw new NotImplementedException();
        }
    }
}