using Autofac;
using FashionStore.Application.Services;
using FashionStore.Application.Services.Interfaces;

namespace FashionStore.Application.Bootstrapper.InversionOfControl.Modules
{
    public class ServiceAppModule : global::Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType(typeof(AdminAppService)).As(typeof(IAdminAppService)).InstancePerRequest();
            
        }
    }
}