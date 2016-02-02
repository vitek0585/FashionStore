using System.Security.Claims;
using System.Web.Helpers;
using System.Web.Http;
using System.Web.Mvc;
using Autofac.Integration.Mvc;
using Autofac.Integration.WebApi;
using FashionStore;
using FashionStore.Application.Bootstrapper.InversionOfControl;
using FashionStore.Application.Bootstrapper.InversionOfControl.Modules;
using FashionStore.Configuration.IoC.Module;
using FashionStore.Infastructure.Data.Identity.StartUp;
using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(StartIdentity))]

namespace FashionStore
{
    public class StartIdentity
    {
        public void Configuration(IAppBuilder app)
        {
            var config = new StartUpApp();
            config.ConfigureAuth(app, "/Account/Login");
            IoC.RegisterModule(new AnyModule());
            IoC.RegisterModule(new IdentityModule(app));

            app.UseAutofacMvc();
            app.UseAutofacMiddleware(IoC.Scope);

            DependencyResolver.SetResolver(new AutofacDependencyResolver(IoC.Scope));
            GlobalConfiguration.Configuration.DependencyResolver = new AutofacWebApiDependencyResolver(IoC.Scope);

            AntiForgeryConfig.UniqueClaimTypeIdentifier = ClaimTypes.NameIdentifier;
        }
    }
}
