using Autofac;
using Microsoft.Extensions.Caching.Memory;
using System;
using WebApi.Providers;
using WebApi.Interface;

namespace WebApi.Core
{
    public class Ioc
    {
        private static IContainer container;
        public static void Register(string[] args)
        {
            var builder = new ContainerBuilder();

            builder.RegisterInstance<SystemConfig>(SystemConfig.Reload())
                .As<ISystemConfig>()
                .SingleInstance();
            builder.RegisterType<CameraProvider>()
              .As<ICameraProvider>()
              .SingleInstance();
            builder.RegisterInstance(new MemoryCache(new MemoryCacheOptions()));

            container = builder.Build();

        }
        public static T Get<T>() where T : class
        {
            if (typeof(T).IsInterface == false)
            {
                throw new Exception("T must be interface");
            }
            return container.Resolve<T>();
        }
        public static ISystemConfig GetConfig()
        {
            return Get<ISystemConfig>();
        }
        public static MemoryCache GetCache()
        {
            return container.Resolve<MemoryCache>();
        }
    }
}
