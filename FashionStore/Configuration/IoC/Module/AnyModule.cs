using Autofac;
using FashionStore.Models.Order;
using FashionStore.WorkFlow.BreadCrumbs.Tag;
using FashionStore.WorkFlow.BreadCrumbs.Tag.Interfaces;
using FashionStore.WorkFlow.Cart;
using FashionStore.WorkFlow.Cart.Interfaces.Provider;
using FashionStore.WorkFlow.UserSession;
using FashionStore.WorkFlow.UserSession.Interfaces;
using FashionStore.WorkFlow.ViewedStorage.Provider;
using FashionStore.WorkFlow.ViewedStorage.Provider.Interface;
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
            builder.RegisterType<CookieConsumer>().As<ICookieConsumer>().InstancePerRequest();
            builder.RegisterType<CartProvider>().As<ICartProvider<UserOrderModel>>().InstancePerLifetimeScope();
            builder.RegisterType<LogWebSql>().As<ILogWriter<string>>().InstancePerRequest();
            builder.RegisterType<TagCreator>().As<ITagCreator>().InstancePerRequest();
            builder.RegisterType<RecentlyViewedProvider>().As<IRecentlyViewedProvider>().InstancePerRequest();
            builder.RegisterType<UserSession>().As<IClearUserSession>().InstancePerRequest();



        } 
    }
}