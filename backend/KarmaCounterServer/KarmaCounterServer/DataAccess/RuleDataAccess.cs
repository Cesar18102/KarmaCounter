using System.Data.Common;
using System.Threading.Tasks;
using System.Collections.Generic;

using Autofac;

using DbUtil;

using KarmaCounterServer.Model;
using KarmaCounterServer.ModelMapping;
using KarmaCounterServer.ModelMapping.AttributeTemplates;
using KarmaCounterServer.ModelMapping.AttributeTemplates.Rule;

namespace KarmaCounterServer.DataAccess
{
    public class RuleDataAccess : DataAccess<Rule>
    {
        public override async Task<Rule> Create(Rule model)
        {
            IRepoFactory repoFactory = Global.DI.Resolve<IRepoFactory>();
            ModelMapper mapper = Global.DI.Resolve<ModelMapper>();
            Rule newRule = null;

            using (DbConnection connection = repoFactory.GetConnection())
            {
                DbMappingInfo ruleInsertInfo = mapper.MapFromModel< Rule, TableAttribute, RuleInsert>(model);

                (string cmdText, List<(string key, object val)> par) cmdInsertInfo = ruleInsertInfo.CreateInsertText();
                DbCommand cmd = CreateCommand(cmdInsertInfo.cmdText, connection, repoFactory, cmdInsertInfo.par);

                await connection.OpenAsync();

                using (DbDataReader reader = await cmd.ExecuteReaderAsync())
                    newRule = mapper.MapToModelSingle<Rule, TableAttribute, RuleSelectInserted>(reader);
            }

            return await GetById(newRule.Id);
        }

        public async Task<Rule> GetById(long id)
        {
            IRepoFactory repoFactory = Global.DI.Resolve<IRepoFactory>();
            ModelMapper mapper = Global.DI.Resolve<ModelMapper>();

            using (DbConnection connection = repoFactory.GetConnection())
            {
                DbMappingInfo ruleSelectInfo = mapper.MapFromModel<Rule, TableAttribute, RuleSelect, RuleSelectForeign, RuleSelectWhere>(new Rule(id));

                (string cmdText, List<(string key, object val)> par) cmdSelectInfo = ruleSelectInfo.CreateSelectText();
                DbCommand cmdSelect = CreateCommand(cmdSelectInfo.cmdText, connection, repoFactory, cmdSelectInfo.par);

                await connection.OpenAsync();

                using (DbDataReader reader = await cmdSelect.ExecuteReaderAsync())
                    return mapper.MapToModelSingle<Rule, TableAttribute, RuleSelect, RuleSelectForeign>(reader);
            }
        }

        public async Task<List<Rule>> GetByGroup(long groupId)
        {
            IRepoFactory repoFactory = Global.DI.Resolve<IRepoFactory>();
            ModelMapper mapper = Global.DI.Resolve<ModelMapper>();

            using (DbConnection connection = repoFactory.GetConnection())
            {
                DbMappingInfo ruleSelectInfo = mapper.MapFromModel<Rule, TableAttribute, RuleSelect, RuleSelectForeign, RuleSelectWhereGroup>(new Rule(new Group(groupId)));

                (string cmdText, List<(string key, object val)> par) cmdSelectInfo = ruleSelectInfo.CreateSelectText();
                DbCommand cmdSelect = CreateCommand(cmdSelectInfo.cmdText, connection, repoFactory, cmdSelectInfo.par);

                await connection.OpenAsync();

                using (DbDataReader reader = await cmdSelect.ExecuteReaderAsync())
                    return mapper.MapToModel<Rule, TableAttribute, RuleSelect, RuleSelectForeign>(reader);
            }
        }

        public override Task<Rule> Delete(Rule model)
        {
            throw new System.NotImplementedException();
        }

        public override Task<Rule> Update(Rule model)
        {
            throw new System.NotImplementedException();
        }
    }
}