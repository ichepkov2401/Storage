﻿using Autofac;

namespace Storage.View
{
    internal class AutofacIntegration
    {
        static IContainer Container;
        static AutofacIntegration()
        {
            var builder = new ContainerBuilder();
            builder.RegisterModule(new AutofacConfig());
            Container = builder.Build();
        }
        public static T GetInstance<T>()
        {
            using (var scope = Container.BeginLifetimeScope())
            {
                return scope.Resolve<T>();
            }
        }
    }
}
