using System;
using System.Web.Mvc;
using Autofac;
using FashionStore.Domain.Interfaces.Repository;
using FashionStore.Infastructure.Data.Service.Store;
using FashionStore.Infastructure.Data.Service.UoF;
using FashionStore.Service.Interfaces.Services;
using FashionStore.Service.Interfaces.UoW;

namespace FashionStore.Application.Bootstrapper.InversionOfControl.Modules
{
    public class ServiceModule : global::Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<UnitOfWorkStore>().As<IUnitOfWorkStore>().InstancePerRequest();

            builder.RegisterType(typeof(GoodService)).As(typeof(IGoodService)).InstancePerRequest();
            builder.RegisterType(typeof(CategoryService)).As(typeof(ICategoryService)).InstancePerRequest();
            builder.RegisterType<PurchaseService>().As<IPurchaseService>().InstancePerRequest();

            UriBuilder uriBuilder = new UriBuilder();
            uriBuilder.Scheme = "https";
            uriBuilder.Host = "api.privatbank.ua";
            uriBuilder.Path = "p24api/pubinfo";
            uriBuilder.Query = "json&exchange&coursid=5";

            builder.Register(i => new ExchangeRatesService(DependencyResolver.Current.GetService<IUnitOfWorkStore>(),
                DependencyResolver.Current.GetService<IExchangeRatesRepository>(), uriBuilder.ToString()))
                .As<IExchangeRatesService>().InstancePerRequest();

            builder.RegisterType<SaleService>().As<ISaleService>().InstancePerRequest();
        }
    }
}