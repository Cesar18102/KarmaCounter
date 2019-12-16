using System;
using System.Linq;
using System.Data.Common;
using System.Threading.Tasks;
using System.Collections.Generic;

using DbUtil;

using Autofac;

using KarmaCounterServer.Model;
using KarmaCounterServer.ModelMapping;
using KarmaCounterServer.Dto;
using KarmaCounterServer.ModelMapping.AttributeTemplates;
using KarmaCounterServer.ModelMapping.AttributeTemplates.User;

namespace KarmaCounterServer.DataAccess
{
    public class UserDataAccess : DataAccess<User>
    {
        public override async Task<User> Create(User user)
        {
            IRepoFactory repoFactory = Global.DI.Resolve<IRepoFactory>();
            ModelMapper mapper = Global.DI.Resolve<ModelMapper>();
            User newUser = null;

            using (DbConnection connection = repoFactory.GetConnection())
            {
                DbMappingInfo userInsertInfo = mapper.MapFromModel<User, TableAttribute, UserInsert>(user);

                (string cmdText, List<(string key, object val)> par) cmdInsertInfo = userInsertInfo.CreateInsertText();
                DbCommand cmd = CreateCommand(cmdInsertInfo.cmdText, connection, repoFactory, cmdInsertInfo.par);

                await connection.OpenAsync();

                using (DbDataReader reader = await cmd.ExecuteReaderAsync())
                    newUser = mapper.MapToModelSingle<User, TableAttribute, UserSelectInserted>(reader);
            }

            return await GetById(newUser.Id);
        }

        public async Task<bool> CheckPassword(LoginForm loginForm, int storedHashLen = 64, string hashfunc = "MD5", int hashLen = 32)
        {
            IRepoFactory repoFactory = Global.DI.Resolve<IRepoFactory>();

            using (DbConnection connection = repoFactory.GetConnection())
            {
                DbCommand cmd = repoFactory.CreateCommand($"SELECT CAST(CASE WHEN CONVERT(CHAR({hashLen}), " +
                                                                      $"HashBytes('{hashfunc}', CONCAT(" +
                                                                                    $"CONVERT(CHAR({storedHashLen}), pwd), " +
                                                                                    $"CONVERT(CHAR({loginForm.Seed.Length}), @seed)" +
                                                                                ")" +
                                                                      "), 2) = UPPER(@pwd) THEN 1 ELSE 0 END AS BIT) AS result " +
                                                          $"FROM users WHERE login = @login;", connection);

                cmd.Parameters.Add(repoFactory.CreateParameter("@seed", loginForm.Seed));
                cmd.Parameters.Add(repoFactory.CreateParameter("@seedlen", loginForm.Seed.Length));
                cmd.Parameters.Add(repoFactory.CreateParameter("@pwd", loginForm.Password));
                cmd.Parameters.Add(repoFactory.CreateParameter("@login", loginForm.Login));

                await connection.OpenAsync();
                return Global.DI.Resolve<ModelMapper>().MapToModelSingle<BoolResult, DbMappingTableAttribute, DbMappingAttribute>(await cmd.ExecuteReaderAsync()).Result;
            }
        }

        public async Task<User> GetByLogin(string login)
        {
            IRepoFactory repoFactory = Global.DI.Resolve<IRepoFactory>();
            ModelMapper mapper = Global.DI.Resolve<ModelMapper>();

            using (DbConnection connection = repoFactory.GetConnection())
            {
                DbMappingInfo userSelectInfo = mapper.MapFromModel<User, TableAttribute, UserSelect, ForeignIgnore, UserSelectWhereLogin>(new User(login));

                (string cmdText, List<(string key, object val)> par) cmdSelectInfo = userSelectInfo.CreateSelectText();
                DbCommand cmdSelect = CreateCommand(cmdSelectInfo.cmdText, connection, repoFactory, cmdSelectInfo.par);

                await connection.OpenAsync();

                using (DbDataReader reader = await cmdSelect.ExecuteReaderAsync())
                    return mapper.MapToModelSingle<User, TableAttribute, UserSelect>(reader);
            }
        }

        public async Task<User> GetById(long id)
        {
            IRepoFactory repoFactory = Global.DI.Resolve<IRepoFactory>();
            ModelMapper mapper = Global.DI.Resolve<ModelMapper>();

            using (DbConnection connection = repoFactory.GetConnection())
            {
                DbMappingInfo userSelectInfo = mapper.MapFromModel<User, TableAttribute, UserSelect, ForeignIgnore, UserSelectWhere>(new User(id));

                (string cmdText, List<(string key, object val)> par) cmdSelectInfo = userSelectInfo.CreateSelectText();
                DbCommand cmdSelect = CreateCommand(cmdSelectInfo.cmdText, connection, repoFactory, cmdSelectInfo.par);

                await connection.OpenAsync();

                using (DbDataReader reader = await cmdSelect.ExecuteReaderAsync())
                    return mapper.MapToModelSingle<User, TableAttribute, UserSelect>(reader);
            }
        }

        public override async Task<User> Update(User model)
        {
            IRepoFactory repoFactory = Global.DI.Resolve<IRepoFactory>();
            ModelMapper mapper = Global.DI.Resolve<ModelMapper>();

            using (DbConnection connection = repoFactory.GetConnection())
            {
                DbMappingInfo userUpdateInfo = mapper.MapFromModel<User, TableAttribute, UserUpdate, ForeignIgnore, UserSelectWhere>(model);

                (string cmdText, List<(string key, object val)> par) cmdUpdateInfo = userUpdateInfo.CreateUpdateText();
                DbCommand cmdUpdate = CreateCommand(cmdUpdateInfo.cmdText, connection, repoFactory, cmdUpdateInfo.par);

                await connection.OpenAsync();
                await cmdUpdate.ExecuteNonQueryAsync();
            }

            return await GetById(model.Id);
        }

        public override async Task<User> Delete(User model)
        {
            throw new NotImplementedException();
        }
    }
}