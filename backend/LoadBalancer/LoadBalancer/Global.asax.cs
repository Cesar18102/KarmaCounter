using System;
using System.Web;
using System.Web.Http;

using Autofac;

using LoadBalancer.Log;
using LoadBalancer.Models;

namespace LoadBalancer
{
    public class Global : HttpApplication
    {
        public static IContainer DI { get; private set; }

        private void InitServices()
        {
            ContainerBuilder builder = new ContainerBuilder();

            builder.RegisterType<Balancer>().SingleInstance().AsSelf();
            builder.RegisterType<FileLogger>().SingleInstance().As<ILogger>().UsingConstructor(new Type[] { typeof(string) });

            DI = builder.Build();
        }

        protected void Application_Start(object sender, EventArgs e)
        {
            GlobalConfiguration.Configure(WebApiConfig.Register);
            InitServices();

            ILogger logger = DI.Resolve<ILogger>(new TypedParameter(typeof(string), "C:/AspServer/balancer/Log"));
            logger.Debug("App started");
        }
    }
}