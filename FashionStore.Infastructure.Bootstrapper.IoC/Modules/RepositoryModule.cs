using Autofac;
using FashionStore.Domain.Interfaces.Repository;
using FashionStore.Infastructure.Data.Repository.Store;

namespace FashionStore.Application.Bootstrapper.InversionOfControl.Modules
{
    public class RepositoryModule : global::Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType(typeof(GoodsRepository)).As(typeof(IGoodsRepository)).InstancePerRequest();
            builder.RegisterType(typeof(CategoryRepository)).As(typeof(ICategoryRepository)).InstancePerRequest();
            builder.RegisterType(typeof(CategoryTypeRepository)).As(typeof(ICategoryTypeRepository)).InstancePerRequest();
            builder.RegisterType(typeof(SaleRepository)).As(typeof(ISaleRepository)).InstancePerRequest();
            builder.RegisterType(typeof(SalePosRepository)).As(typeof(ISalePosRepository)).InstancePerRequest();
            builder.RegisterType<ClassificationGoodRepository>().As<IClassificationGoodRepository>()
                .InstancePerLifetimeScope();
            builder.RegisterType<ColorRepository>().As<IColorRepository>().InstancePerRequest();
            builder.RegisterType<SizeRepository>().As<ISizeRepository>().InstancePerRequest();
            builder.RegisterType<ImageRepository>().As<IImageRepository>().InstancePerRequest();

            builder.RegisterType<ExchangeRatesRepository>().As<IExchangeRatesRepository>().InstancePerRequest();

        }
    }
}