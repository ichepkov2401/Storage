using Autofac;
using AutoMapper.Contrib.Autofac.DependencyInjection;
using Storage.Bl.Service;
using Storage.Bl.Service.Interfaces;
using Storage.Data.Repositories;

namespace Storage.View.Configuration
{
    internal class AutofacConfig : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            switch (Config.Configuration["ConnectionStrings:Provider"])
            {
                case ProviderConstants.Json:
                    builder.RegisterType<StorageFileRepository>().As<IStorageRepository>().WithParameter("connection", Config.Configuration["ConnectionStrings:Connection"]);
                    break;
                case ProviderConstants.SqLite:
                default:
                    builder.RegisterType<StorageRepository>().As<IStorageRepository>().WithParameter("connection", Config.Configuration["ConnectionStrings:Connection"]);
                    break;
            }
            builder.RegisterType<BoxService>().As<IBoxService>();
            builder.RegisterType<GeneratorService>().As<IGeneratorService>();
            builder.RegisterType<PalletService>().As<IPalletService>();
            builder.RegisterAutoMapper(typeof(PalletService).Assembly);
        }
    }
}
