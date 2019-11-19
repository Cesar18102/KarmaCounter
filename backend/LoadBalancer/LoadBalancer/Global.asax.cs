using System;
using System.Web;
using System.Linq;
using System.Web.Http;
using System.Web.Hosting;

using Autofac;
using Autofac.Core;

using IniParser;

using DbUtil;

using Logger;
using Logger.Impl;
using Logger.Util;

using LoadBalancer.Models;

namespace LoadBalancer
{
    public class Global : HttpApplication
    {
        public static IContainer DI { get; private set; }

        private const string SERVERS_INI_FILE_PATH = "bin/servers.ini";
        private const string SERVERS_INI_SECTION = "servers";

        private const string LOGGER_DB_SERVER = "karmaloadbalancerdb.mssql.somee.com";
        private const string LOGGER_DB = "karmaloadbalancerdb";
        private const string LOGGER_DB_LOGIN = "KarmaLBalancer_SQLLogin_1";
        private const string LOGGER_DB_PWD = "q2ezdduggr";

        private const string LOG_TABLE_NAME = "log";
        private const string LOG_TABLE_CAPTION_FIELD = "caption";
        private const string LOG_TABLE_DATE_TIME_FIELD = "date_time_code";
        private const string LOG_TABLE_MESSAGE_FIELD = "message";

        private void InitServices()
        {
            ContainerBuilder builder = new ContainerBuilder();

            builder.RegisterType<Balancer>().SingleInstance().AsSelf().
                UsingConstructor(new Type[] { typeof(string[]) }).
                WithParameters(new Parameter[] {
                    new TypedParameter(typeof(string[]), new FileIniDataParser().ReadFile(HostingEnvironment.MapPath("~") + SERVERS_INI_FILE_PATH)[SERVERS_INI_SECTION].Select(S => S.Value).ToArray())
                });

            builder.RegisterType<MsSqlDbLogger>().SingleInstance().As<ILogger>().
                UsingConstructor(new Type[] { typeof(MsSqlRepoFactory), typeof(DbLogConfig) }).
                WithParameters(new Parameter[] {
                    new TypedParameter(typeof(MsSqlRepoFactory), new MsSqlRepoFactory(LOGGER_DB_SERVER, LOGGER_DB, LOGGER_DB_LOGIN, LOGGER_DB_PWD)),
                    new TypedParameter(typeof(DbLogConfig), new DbLogConfig(LOG_TABLE_NAME, LOG_TABLE_CAPTION_FIELD, LOG_TABLE_DATE_TIME_FIELD, LOG_TABLE_MESSAGE_FIELD))
                });

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