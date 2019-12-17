using Autofac;

using KarmaCounter.Server.Interaction;
using KarmaCounter.LocalDataAccess;
using KarmaCounter.ResponseParser;
using KarmaCounter.Controllers;
using KarmaCounter.Models;
using KarmaCounter.Util;

namespace KarmaCounter
{
    public static class DI
    {
        public static IContainer Services { get; private set; }

        public static void Init()
        {
            ContainerBuilder builder = new ContainerBuilder();

            builder.RegisterType<SessionWrapper>().SingleInstance().AsSelf();
            builder.RegisterType<LogicServerWrapper>().SingleInstance().AsSelf();
            builder.RegisterType<JsonResponseParser>().SingleInstance().As<IResponseParser>();
            builder.RegisterType<FileManager>().SingleInstance().AsSelf();
            builder.RegisterType<QR>().SingleInstance().AsSelf();

            builder.RegisterType<AuthController>().SingleInstance().AsSelf();

            Services = builder.Build();
        }
    }
}
