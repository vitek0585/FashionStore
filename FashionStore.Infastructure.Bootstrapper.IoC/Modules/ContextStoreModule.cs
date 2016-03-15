using Autofac;
using FashionStore.Infrastructure.Data.Context.Store.Context;

namespace FashionStore.Application.Bootstrapper.InversionOfControl.Modules
{
    public class ContextStoreModule : global::Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType(typeof(ShopContext)).AsSelf().InstancePerLifetimeScope();
        }

    }

}