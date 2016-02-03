
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
        public static IContainer Scope
        {
            get
            {
                return NestedContainer.Scope ?? (NestedContainer.Scope = NestedContainer.Builder.Build());
                
            }
        }

        private class NestedContainer
        {
            public static ContainerBuilder Builder { get; set; }
            public static IContainer Scope { get; set; }

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
        
    }
}