using Autofac;
using FashionStore.Domain.Interfaces.Repository;
using FashionStore.Infastructure.Data.Repository.Store;

namespace FashionStore.Application.Bootstrapper.InversionOfControl.Modules
{
    public class RepositoryModule : global::Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType(typeof(GoodsRepository)).As(typeof(IGoodsRepository)).InstancePerLifetimeScope();
            builder.RegisterType(typeof(CategoryRepository)).As(typeof(ICategoryRepository)).InstancePerLifetimeScope();
            builder.RegisterType(typeof(CategoryTypeRepository)).As(typeof(ICategoryTypeRepository)).InstancePerLifetimeScope();
            builder.RegisterType(typeof(SaleRepository)).As(typeof(ISaleRepository)).InstancePerLifetimeScope();
            builder.RegisterType(typeof(SalePosRepository)).As(typeof(ISalePosRepository)).InstancePerLifetimeScope();
            builder.RegisterType<ClassificationGoodRepository>().As<IClassificationGoodRepository>()
                .InstancePerLifetimeScope();
            builder.RegisterType<ColorRepository>().As<IColorRepository>().InstancePerLifetimeScope();
            builder.RegisterType<SizeRepository>().As<ISizeRepository>().InstancePerLifetimeScope();


            builder.RegisterType<ExchangeRatesRepository>().As<IExchangeRatesRepository>().InstancePerLifetimeScope();

        }
    }
}