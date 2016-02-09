
using System;
using System.Reflection;
using Autofac;
using Autofac.Core;
using Autofac.Integration.Mvc;
using Autofac.Integration.WebApi;
using FashionStore.Application.Bootstrapper.InversionOfControl.Modules;

namespace FashionStore.Application.Bootstrapper.InversionOfControl
{
    public class IoC
    {
        private static Func<Type, object> _resolver;

        public static T Resolve<T>()
        {
            return (T)_resolver(typeof(T));
        }
        private class NestedContainer
        {
            public static ContainerBuilder Builder { get; private set; }

            static NestedContainer()
            {
                Builder = new ContainerBuilder();

                RegisterModule(new ContextStoreModule());
                RegisterModule(new RepositoryModule());
                RegisterModule(new ServiceModule());
            }
        }
        public static void RegisterControllers(Assembly assembly)
        {
            NestedContainer.Builder.RegisterControllers(assembly).PropertiesAutowired();
            NestedContainer.Builder.RegisterApiControllers(assembly);
        }

        public static void RegisterModule(IModule module)
        {
            NestedContainer.Builder.RegisterModule(module);
        }

        public static void ActionResolverMvc(Func<Type, object> resolver)
        {
            _resolver = resolver;
        }

        public static IContainer BuildContainer()
        {
            return NestedContainer.Builder.Build();
        }
       
    }
}