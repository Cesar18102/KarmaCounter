using System.Data.Common;
using System.Threading.Tasks;
using System.Collections.Generic;

using Autofac;

using DbUtil;

using KarmaCounterServer.Model;
using KarmaCounterServer.ModelMapping;
using KarmaCounterServer.ModelMapping.AttributeTemplates;
using KarmaCounterServer.ModelMapping.AttributeTemplates.Invitation;

namespace KarmaCounterServer.DataAccess
{
    public class InvitationDataAccess : DataAccess<Invitation>
    {
        public override async Task<Invitation> Create(Invitation model)
        {
            IRepoFactory repoFactory = Global.DI.Resolve<IRepoFactory>();
            ModelMapper mapper = Global.DI.Resolve<ModelMapper>();
            Invitation newInvitation = null;

            using (DbConnection connection = repoFactory.GetConnection())
            {
                DbMappingInfo invitationInsertInfo = mapper.MapFromModel<Invitation, TableAttribute, InvitationInsert>(model);

                (string cmdText, List<(string key, object val)> par) cmdInsertInfo = invitationInsertInfo.CreateInsertText();
                DbCommand cmd = CreateCommand(cmdInsertInfo.cmdText, connection, repoFactory, cmdInsertInfo.par);

                await connection.OpenAsync();

                using (DbDataReader reader = await cmd.ExecuteReaderAsync())
                    newInvitation = mapper.MapToModelSingle<Invitation, TableAttribute, InvitationSelectInserted>(reader);
            }

            return await GetById(newInvitation.Id);
        }

        public async Task<Invitation> GetById(long id)
        {
            IRepoFactory repoFactory = Global.DI.Resolve<IRepoFactory>();
            ModelMapper mapper = Global.DI.Resolve<ModelMapper>();

            using (DbConnection connection = repoFactory.GetConnection())
            {
                DbMappingInfo invitationSelectInfo = mapper.MapFromModel<Invitation, TableAttribute, InvitationSelect, InvitationSelectForeign, InvitationSelectWhere>(new Invitation(id));

                (string cmdText, List<(string key, object val)> par) cmdSelectInfo = invitationSelectInfo.CreateSelectText();
                DbCommand cmd = CreateCommand(cmdSelectInfo.cmdText, connection, repoFactory, cmdSelectInfo.par);

                await connection.OpenAsync();

                using (DbDataReader reader = await cmd.ExecuteReaderAsync())
                    return mapper.MapToModelSingle<Invitation, TableAttribute, InvitationSelect, InvitationSelectForeign>(reader);
            }
        }

        public async Task<List<Invitation>> GetByUserId(long id)
        {
            IRepoFactory repoFactory = Global.DI.Resolve<IRepoFactory>();
            ModelMapper mapper = Global.DI.Resolve<ModelMapper>();

            using (DbConnection connection = repoFactory.GetConnection())
            {
                DbMappingInfo invitationSelectInfo = mapper.MapFromModel<Invitation, TableAttribute, InvitationSelect, InvitationSelectForeign, InvitationSelectWhereUser>(new Invitation(new User(id)));

                (string cmdText, List<(string key, object val)> par) cmdSelectInfo = invitationSelectInfo.CreateSelectText();
                DbCommand cmdSelect = CreateCommand(cmdSelectInfo.cmdText, connection, repoFactory, cmdSelectInfo.par);

                await connection.OpenAsync();

                using (DbDataReader reader = await cmdSelect.ExecuteReaderAsync())
                    return mapper.MapToModel<Invitation, TableAttribute, InvitationSelect, InvitationSelectForeign>(reader);
            }
        }

        public override Task<Invitation> Delete(Invitation model)
        {
            throw new System.NotImplementedException();
        }

        public override Task<Invitation> Update(Invitation model)
        {
            throw new System.NotImplementedException();
        }
    }
}