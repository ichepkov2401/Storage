using Autofac;
using AutoMapper;

namespace Storage.View.Configuration
{
    internal class AutofacIntegration
    {
        static IContainer Container;
        static AutofacIntegration()
        {
            var builder = new ContainerBuilder();
            builder.RegisterModule(new AutofacConfig());
            Container = builder.Build();
            var mapperConfiguration = Container.Resolve<MapperConfiguration>();
#if DEBUG
            try
            {
                mapperConfiguration.AssertConfigurationIsValid();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
#endif
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
