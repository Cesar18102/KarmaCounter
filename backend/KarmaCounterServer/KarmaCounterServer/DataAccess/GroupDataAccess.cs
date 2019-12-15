using System.Data.Common;
using System.Threading.Tasks;
using System.Collections.Generic;

using Autofac;

using DbUtil;

using KarmaCounterServer.Model;
using KarmaCounterServer.ModelMapping;
using KarmaCounterServer.ModelMapping.AttributeTemplates;
using KarmaCounterServer.ModelMapping.AttributeTemplates.Group;
using System;
using System.Collections;

namespace KarmaCounterServer.DataAccess
{
    public class GroupDataAccess : DataAccess<Group>
    {
        public override async Task<Group> Create(Group model)
        {
            IRepoFactory repoFactory = Global.DI.Resolve<IRepoFactory>();
            ModelMapper mapper = Global.DI.Resolve<ModelMapper>();
            Group newGroup = null;

            using (DbConnection connection = repoFactory.GetConnection())
            {
                DbMappingInfo groupInsertInfo = mapper.MapFromModel<Group, TableAttribute, GroupInsert, GroupInsertForeign, GroupInsert>(model);

                (string cmdText, List<(string key, object val)> par) cmdInsertInfo = groupInsertInfo.CreateInsertText();
                DbCommand cmd = CreateCommand(cmdInsertInfo.cmdText, connection, repoFactory, cmdInsertInfo.par);

                await connection.OpenAsync();

                using (DbDataReader reader = await cmd.ExecuteReaderAsync())
                    newGroup = mapper.MapToModelSingle<Group, TableAttribute, GroupSelectInserted, ForeignIgnore>(reader);
            }

            return await GetById(newGroup.Id);
        }

        public async Task<List<Group>> GetAll()
        {
            IRepoFactory repoFactory = Global.DI.Resolve<IRepoFactory>();
            ModelMapper mapper = Global.DI.Resolve<ModelMapper>();

            using (DbConnection connection = repoFactory.GetConnection())
            {
                DbMappingInfo groupSelectInfo = mapper.MapFromModel<Group, TableAttribute, GroupSelect, GroupSelectForeign>();

                (string cmdText, List<(string key, object val)> par) cmdSelectInfo = groupSelectInfo.CreateSelectText();
                DbCommand cmd = CreateCommand(cmdSelectInfo.cmdText, connection, repoFactory, cmdSelectInfo.par);

                await connection.OpenAsync();

                using (DbDataReader reader = await cmd.ExecuteReaderAsync())
                    return mapper.MapToModel<Group, TableAttribute, GroupSelect, GroupSelectForeign>(reader);
            }
        }

        public async Task<List<Group>> GetByOwner(User owner)
        {
            IRepoFactory repoFactory = Global.DI.Resolve<IRepoFactory>();
            ModelMapper mapper = Global.DI.Resolve<ModelMapper>();

            using (DbConnection connection = repoFactory.GetConnection())
            {
                DbMappingInfo groupSelectInfo = mapper.MapFromModel<Group, TableAttribute, GroupSelect, GroupSelectForeign, GroupSelectWhereOwner>(new Group(new Ownership(owner)));

                (string cmdText, List<(string key, object val)> par) cmdSelectInfo = groupSelectInfo.CreateSelectText();
                DbCommand cmd = CreateCommand(cmdSelectInfo.cmdText, connection, repoFactory, cmdSelectInfo.par);

                await connection.OpenAsync();

                using (DbDataReader reader = await cmd.ExecuteReaderAsync())
                    return mapper.MapToModel<Group, TableAttribute, GroupSelect, GroupSelectForeign>(reader);
            }
        }

        public async Task<Group> GetById(long id)
        {
            IRepoFactory repoFactory = Global.DI.Resolve<IRepoFactory>();
            ModelMapper mapper = Global.DI.Resolve<ModelMapper>();
            Group group = null;

            using (DbConnection connection = repoFactory.GetConnection())
            {
                DbMappingInfo newGroupMappringInfo = mapper.MapFromModel<Group, TableAttribute, GroupSelect, GroupSelectForeign, GroupSelectWhere>(new Group(id));

                (string cmdText, List<(string key, object val)> par) cmdSelectInfo = newGroupMappringInfo.CreateSelectText();
                DbCommand cmdSelect = CreateCommand(cmdSelectInfo.cmdText, connection, repoFactory, cmdSelectInfo.par);

                await connection.OpenAsync();

                using (DbDataReader reader = await cmdSelect.ExecuteReaderAsync())
                    group = mapper.MapToModelSingle<Group, TableAttribute, GroupSelect, GroupSelectForeign>(reader);
            }

            foreach (Membership member in await Global.DI.Resolve<MembershipDataAccess>().GetGroupMemberships(id))
                group.Members.Add(member);

            return group;
        }

        public override Task<Group> Delete(Group model)
        {
            throw new System.NotImplementedException();
        }

        public override Task<Group> Update(Group model)
        {
            throw new System.NotImplementedException();
        }
    }
}