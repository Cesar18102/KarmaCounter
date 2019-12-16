using System.Data.Common;
using System.Threading.Tasks;
using System.Collections.Generic;

using DbUtil;

using Autofac;

using KarmaCounterServer.Model;
using KarmaCounterServer.ModelMapping;
using KarmaCounterServer.ModelMapping.AttributeTemplates;
using KarmaCounterServer.ModelMapping.AttributeTemplates.Action;

namespace KarmaCounterServer.DataAccess
{
    public class RuleActionDataAccess : DataAccess<RuleAction>
    {
        public override async Task<RuleAction> Create(RuleAction model)
        {
            IRepoFactory repoFactory = Global.DI.Resolve<IRepoFactory>();
            ModelMapper mapper = Global.DI.Resolve<ModelMapper>();
            RuleAction newAction = null;

            using (DbConnection connection = repoFactory.GetConnection())
            {
                DbMappingInfo actionInsertInfo = mapper.MapFromModel<RuleAction, TableAttribute, ActionInsert>(model);

                (string cmdText, List<(string key, object val)> par) cmdInsertInfo = actionInsertInfo.CreateInsertText();
                DbCommand cmd = CreateCommand(cmdInsertInfo.cmdText, connection, repoFactory, cmdInsertInfo.par);

                await connection.OpenAsync();

                using (DbDataReader reader = await cmd.ExecuteReaderAsync())
                    newAction = mapper.MapToModelSingle<RuleAction, TableAttribute, ActionSelectInserted>(reader);
            }

            return await GetById(newAction.Id);
        }

        public async Task<RuleAction> GetById(long id)
        {
            IRepoFactory repoFactory = Global.DI.Resolve<IRepoFactory>();
            ModelMapper mapper = Global.DI.Resolve<ModelMapper>();

            using (DbConnection connection = repoFactory.GetConnection())
            {
                DbMappingInfo ruleSelectInfo = mapper.MapFromModel<RuleAction, TableAttribute, ActionSelect, ActionSelectForeign, ActionSelectWhere>(new RuleAction(id));

                (string cmdText, List<(string key, object val)> par) cmdSelectInfo = ruleSelectInfo.CreateSelectText();
                DbCommand cmdSelect = CreateCommand(cmdSelectInfo.cmdText, connection, repoFactory, cmdSelectInfo.par);

                await connection.OpenAsync();

                using (DbDataReader reader = await cmdSelect.ExecuteReaderAsync())
                    return mapper.MapToModelSingle<RuleAction, TableAttribute, ActionSelect, ActionSelectForeign>(reader);
            }
        }

        public async Task<List<RuleAction>> GetByUser(User user)
        {
            IRepoFactory repoFactory = Global.DI.Resolve<IRepoFactory>();
            ModelMapper mapper = Global.DI.Resolve<ModelMapper>();

            using (DbConnection connection = repoFactory.GetConnection())
            {
                DbMappingInfo ruleSelectInfo = mapper.MapFromModel<RuleAction, TableAttribute, ActionSelect, ActionSelectForeign, ActionSelectWhereUser>(new RuleAction(user));

                (string cmdText, List<(string key, object val)> par) cmdSelectInfo = ruleSelectInfo.CreateSelectText();
                DbCommand cmdSelect = CreateCommand(cmdSelectInfo.cmdText, connection, repoFactory, cmdSelectInfo.par);

                await connection.OpenAsync();

                using (DbDataReader reader = await cmdSelect.ExecuteReaderAsync())
                    return mapper.MapToModel<RuleAction, TableAttribute, ActionSelect, ActionSelectForeign>(reader);
            }
        }

        public async Task<List<RuleAction>> GetByRule(Rule rule)
        {
            IRepoFactory repoFactory = Global.DI.Resolve<IRepoFactory>();
            ModelMapper mapper = Global.DI.Resolve<ModelMapper>();

            using (DbConnection connection = repoFactory.GetConnection())
            {
                DbMappingInfo ruleSelectInfo = mapper.MapFromModel<RuleAction, TableAttribute, ActionSelect, ActionSelectForeign, ActionSelectWhereRule>(new RuleAction(rule));

                (string cmdText, List<(string key, object val)> par) cmdSelectInfo = ruleSelectInfo.CreateSelectText();
                DbCommand cmdSelect = CreateCommand(cmdSelectInfo.cmdText, connection, repoFactory, cmdSelectInfo.par);

                await connection.OpenAsync();

                using (DbDataReader reader = await cmdSelect.ExecuteReaderAsync())
                    return mapper.MapToModel<RuleAction, TableAttribute, ActionSelect, ActionSelectForeign>(reader);
            }
        }

        public async Task<List<RuleAction>> GetByGroup(Group group)
        {
            IRepoFactory repoFactory = Global.DI.Resolve<IRepoFactory>();
            ModelMapper mapper = Global.DI.Resolve<ModelMapper>();

            using (DbConnection connection = repoFactory.GetConnection())
            {
                DbMappingInfo ruleSelectInfo = mapper.MapFromModel<RuleAction, TableAttribute, ActionSelectByGroup, ActionSelectForeign, ActionSelectWhereGroup>(new RuleAction(new Rule(group)));

                (string cmdText, List<(string key, object val)> par) cmdSelectInfo = ruleSelectInfo.CreateSelectText();
                DbCommand cmdSelect = CreateCommand(cmdSelectInfo.cmdText, connection, repoFactory, cmdSelectInfo.par);

                await connection.OpenAsync();

                using (DbDataReader reader = await cmdSelect.ExecuteReaderAsync())
                    return mapper.MapToModel<RuleAction, TableAttribute, ActionSelectByGroup, ActionSelectForeign>(reader);
            }
        }

        public override Task<RuleAction> Delete(RuleAction model)
        {
            throw new System.NotImplementedException();
        }

        public override Task<RuleAction> Update(RuleAction model)
        {
            throw new System.NotImplementedException();
        }
    }
}