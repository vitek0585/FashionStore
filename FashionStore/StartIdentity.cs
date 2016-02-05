﻿using System.Reflection;
using System.Security.Claims;
using System.Web.Helpers;
using System.Web.Http;
using System.Web.Mvc;
using Autofac.Integration.Mvc;
using Autofac.Integration.WebApi;
using FashionStore;
using FashionStore.Application.Bootstrapper.InversionOfControl;
using FashionStore.Configuration.IoC.Module;
using FashionStore.Infastructure.Data.Identity.StartUp;
using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(StartIdentity))]

namespace FashionStore
{
    public class StartIdentity
    {
        private string _defaultLoginPage = "/Account/Login";
        public void Configuration(IAppBuilder app)
        {
            var config = new StartUpApp();
            config.ConfigureAuth(app, _defaultLoginPage);

            IoC.RegisterModule(new AnyModule());
            IoC.RegisterModule(new IdentityModule(app));
            IoC.RegisterControllers(Assembly.GetExecutingAssembly());

            app.UseAutofacMvc();
            app.UseAutofacMiddleware(IoC.Scope);

            DependencyResolver.SetResolver(new AutofacDependencyResolver(IoC.Scope));
            GlobalConfiguration.Configuration.DependencyResolver = new AutofacWebApiDependencyResolver(IoC.Scope);
            IoC.SetResolver(DependencyResolver.Current.GetService);


            AntiForgeryConfig.UniqueClaimTypeIdentifier = ClaimTypes.NameIdentifier;
        }
    }
}
