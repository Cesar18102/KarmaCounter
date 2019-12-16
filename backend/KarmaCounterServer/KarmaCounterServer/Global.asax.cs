using System;
using System.Web;
using System.Web.Http;

using Autofac;
using Autofac.Core;

using DbUtil;

using Logger;
using Logger.Impl;
using Logger.Util;

using KarmaCounterServer.ModelMapping;
using KarmaCounterServer.DataAccess;
using KarmaCounterServer.Services;

namespace KarmaCounterServer
{
    public class Global : HttpApplication
    {
        public static IContainer DI { get; private set; }

        private const string DB_SERVER = "karmadatadb.mssql.somee.com";
        private const string DB = "karmadatadb";
        private const string DB_LOGIN = "KarmaData_SQLLogin_1";
        private const string DB_PWD = "flk3jz7hpu";

        private const string LOG_TABLE_NAME = "log";
        private const string LOG_TABLE_CAPTION_FIELD = "caption";
        private const string LOG_TABLE_DATE_TIME_FIELD = "date_time_code";
        private const string LOG_TABLE_MESSAGE_FIELD = "message";

        private void InitServices()
        {
            ContainerBuilder builder = new ContainerBuilder();

            builder.RegisterType<MsSqlRepoFactory>().SingleInstance().As<IRepoFactory>().
                UsingConstructor(new Type[] { typeof(string), typeof(string), typeof(string), typeof(string), typeof(int), typeof(bool) }).
                WithParameters(new Parameter[] {
                    new PositionalParameter(0, DB_SERVER),
                    new PositionalParameter(1, DB),
                    new PositionalParameter(2, DB_LOGIN),
                    new PositionalParameter(3, DB_PWD),
                }
            );

            builder.RegisterType<MsSqlDbLogger>().SingleInstance().As<ILogger>().
                UsingConstructor(new Type[] { typeof(MsSqlRepoFactory), typeof(DbLogConfig) }).
                WithParameters(new Parameter[] {
                    new TypedParameter(typeof(MsSqlRepoFactory), new MsSqlRepoFactory(DB_SERVER, DB, DB_LOGIN, DB_PWD)),
                    new TypedParameter(typeof(DbLogConfig), new DbLogConfig(LOG_TABLE_NAME, LOG_TABLE_CAPTION_FIELD, LOG_TABLE_DATE_TIME_FIELD, LOG_TABLE_MESSAGE_FIELD))
                });

            builder.RegisterType<ModelMapper>().SingleInstance().AsSelf();

            builder.RegisterType<UserService>().SingleInstance().AsSelf();
            builder.RegisterType<GroupService>().SingleInstance().AsSelf();
            builder.RegisterType<SessionService>().SingleInstance().AsSelf();
            builder.RegisterType<OwnershipService>().SingleInstance().AsSelf();
            builder.RegisterType<GroupInvitationService>().SingleInstance().AsSelf();
            builder.RegisterType<RuleService>().SingleInstance().AsSelf();

            builder.RegisterType<UserDataAccess>().SingleInstance().AsSelf();
            builder.RegisterType<GroupDataAccess>().SingleInstance().AsSelf();
            builder.RegisterType<OwnershipDataAccess>().SingleInstance().AsSelf();
            builder.RegisterType<MembershipDataAccess>().SingleInstance().AsSelf();
            builder.RegisterType<InvitationDataAccess>().SingleInstance().AsSelf();
            builder.RegisterType<RuleDataAccess>().SingleInstance().AsSelf();
            builder.RegisterType<RuleActionDataAccess>().SingleInstance().AsSelf();

            DI = builder.Build();
        }

        protected async void Application_Start(object sender, EventArgs e)
        {
            InitServices();
            await DI.Resolve<ILogger>().Trace("App started");

            GlobalConfiguration.Configure(WebApiConfig.Register);
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
        }

        private async void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e) =>
            await DI.Resolve<ILogger>().Error(e.ExceptionObject as Exception);
    }
}