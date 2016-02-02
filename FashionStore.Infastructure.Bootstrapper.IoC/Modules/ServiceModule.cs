﻿using System;
using System.Web.Mvc;
using Autofac;
using FashionStore.Domain.Interfaces.Repository;
using FashionStore.Infastructure.Data.Identity.Interfaces.Service;
using FashionStore.Infastructure.Data.Service.Identity;
using FashionStore.Infastructure.Data.Service.Store;
using FashionStore.Service.Interfaces.Services;
using FashionStore.Service.Interfaces.UoW;

namespace FashionStore.Application.Bootstrapper.InversionOfControl.Modules
{
    public class ServiceModule : global::Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType(typeof(GoodService)).As(typeof(IGoodService)).InstancePerLifetimeScope();
            builder.RegisterType(typeof(CategoryService)).As(typeof(ICategoryService)).InstancePerLifetimeScope();
            builder.RegisterType<PurchaseService>().As<IPurchaseService>().InstancePerLifetimeScope();

            UriBuilder uriBuilder = new UriBuilder();
            uriBuilder.Scheme = "https";
            uriBuilder.Host = "api.privatbank.ua";
            uriBuilder.Path = "p24api/pubinfo";
            uriBuilder.Query = "json&exchange&coursid=5";

            builder.Register(i => new ExchangeRatesService(DependencyResolver.Current.GetService<IUnitOfWork>(),
                DependencyResolver.Current.GetService<IExchangeRatesRepository>(), uriBuilder.ToString()))
                .As<IExchangeRatesService>().InstancePerLifetimeScope();

            builder.RegisterType<SaleService>().As<ISaleService>().InstancePerLifetimeScope();
            builder.RegisterType<UserAppService>().As<IUserAppService>().InstancePerLifetimeScope();
        }
    }
}