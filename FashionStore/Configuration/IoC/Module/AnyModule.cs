using Autofac;
using FashionStore.Models.Order;
using FashionStore.WorkFlow.BreadCrumbs.Tag;
using FashionStore.WorkFlow.BreadCrumbs.Tag.Interfaces;
using FashionStore.WorkFlow.Cart;
using FashionStore.WorkFlow.Cart.Interfaces.Provider;
using FashionStore.WorkFlow.Log;
using FashionStore.WorkFlow.UserSession;
using FashionStore.WorkFlow.UserSession.Interfaces;
using FashionStore.WorkFlow.ViewedStorage.Provider;
using FashionStore.WorkFlow.ViewedStorage.Provider.Interface;
using WebCookie;
using WebCookie.Interfaces;
using WebLogger.Abstract.Interface;
using WebLogger.Abstract.Interface.Sql;
using WebLogger.WebLog;

namespace FashionStore.Configuration.IoC.Module
{
    public class AnyModule:global::Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<CookieConsumer>().As<ICookieConsumer>().InstancePerRequest();
            builder.RegisterType<CartProvider>().As<ICartProvider<UserOrderModel>>().InstancePerLifetimeScope();

            #region log

            builder.Register((c) => new LogSql(new RequestContextLog(), "ShopContext")).As<ILogWriterSql>().As<ILogReaderSql>().As<ILog>().InstancePerRequest();

            #endregion


            builder.RegisterType<TagCreator>().As<ITagCreator>().InstancePerRequest();
            builder.RegisterType<RecentlyViewedProvider>().As<IRecentlyViewedProvider>().InstancePerRequest();
            builder.RegisterType<UserSession>().As<IClearUserSession>().InstancePerRequest();



        } 
    }
}