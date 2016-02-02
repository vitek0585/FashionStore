using Autofac;
using FashionStore.Models.Order;
using FashionStore.WorkFlow.Cart;
using FashionStore.WorkFlow.Cart.Interfaces.Provider;
using WebCookie;
using WebCookie.Interfaces;
using WebLogger.Abstract.Interface;
using WebLogger.Concreate;

namespace FashionStore.Configuration.IoC.Module
{
    public class AnyModule:global::Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<CookieConsumer>().As<ICookieConsumer>().InstancePerLifetimeScope();
            builder.RegisterType<CartProvider>().As<ICartProvider<UserOrderModel>>().InstancePerLifetimeScope();
            builder.RegisterType<LogWebSql>().As<ILogWriter<string>>().InstancePerLifetimeScope();
        } 
    }
}