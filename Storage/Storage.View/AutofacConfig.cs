using Autofac;
using AutoMapper.Contrib.Autofac.DependencyInjection;
using Storage.Bl.Service;
using Storage.Bl.Service.Interfaces;
using Storage.Data.Repositories;

namespace Storage.View
{
    internal class AutofacConfig : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<StorageRepository>().As<IStorageRepository>().SingleInstance();
            builder.RegisterType<BoxService>().As<IBoxService>().SingleInstance();
            builder.RegisterType<GeneratorService>().As<IGeneratorService>().SingleInstance();
            builder.RegisterType<PalletService>().As<IPalletService>().SingleInstance();
            builder.RegisterAutoMapper(typeof(Program).Assembly, true);
        }
    }
}
