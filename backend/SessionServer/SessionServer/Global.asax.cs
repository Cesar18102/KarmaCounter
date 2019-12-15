using System;
using System.Web;
using System.Web.Http;

using Autofac;
using Autofac.Core;

using DbUtil;

using Logger;
using Logger.Impl;
using Logger.Util;

using SessionServer.Model;

namespace SessionServer
{
    public class Global : HttpApplication
    {
        public static IContainer DI { get; private set; }

        private const string LOGGER_DB_SERVER = "karmasessionserverdb.mssql.somee.com";
        private const string LOGGER_DB = "karmasessionserverdb";
        private const string LOGGER_DB_LOGIN = "KarmaSServer_SQLLogin_1";
        private const string LOGGER_DB_PWD = "m8gwx72zm1";

        private const string LOG_TABLE_NAME = "log";
        private const string LOG_TABLE_CAPTION_FIELD = "caption";
        private const string LOG_TABLE_DATE_TIME_FIELD = "date_time_code";
        private const string LOG_TABLE_MESSAGE_FIELD = "message";

        private void InitServices()
        {
            ContainerBuilder builder = new ContainerBuilder();

            builder.RegisterType<MsSqlDbLogger>().SingleInstance().As<ILogger>().
                UsingConstructor(new Type[] { typeof(MsSqlRepoFactory), typeof(DbLogConfig) }).
                WithParameters(new Parameter[] {
                    new TypedParameter(typeof(MsSqlRepoFactory), new MsSqlRepoFactory(LOGGER_DB_SERVER, LOGGER_DB, LOGGER_DB_LOGIN, LOGGER_DB_PWD)),
                    new TypedParameter(typeof(DbLogConfig), new DbLogConfig(LOG_TABLE_NAME, LOG_TABLE_CAPTION_FIELD, LOG_TABLE_DATE_TIME_FIELD, LOG_TABLE_MESSAGE_FIELD))
                });

            builder.RegisterType<SessionTable>().SingleInstance().AsSelf();

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