﻿using System.Data.Entity;
using System.Web;
using Autofac;
using FashionStore.Infrastructure.Data.Identity.Context;
using FashionStore.Infrastructure.Data.Identity.Entities;
using FashionStore.Infrastructure.Data.Identity.Interfaces.Service;
using FashionStore.Infrastructure.Data.Identity.Manager;
using FashionStore.Infrastructure.Data.Service.Identity;
using FashionStore.Infrastructure.Data.Service.UoF;
using FashionStore.Service.Interfaces.UoW;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.DataProtection;
using Owin;

namespace FashionStore.Configuration.IoC.Module
{
    public class IdentityModule : global::Autofac.Module
    {
        private IAppBuilder _app;

        public IdentityModule(IAppBuilder app)
        {
            _app = app;
        }

        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<UserAppService>().As<IUserAppService>().InstancePerLifetimeScope();
            builder.RegisterType<RoleService>().As<IRoleService>().InstancePerLifetimeScope();
            builder.RegisterType(typeof(AccountService)).As(typeof(IAccountService)).InstancePerLifetimeScope();
      

            builder.RegisterType(typeof(DbContextIdentity)).AsSelf().InstancePerRequest();
            builder.RegisterType<DbContextIdentity>().As<DbContext>().InstancePerRequest();

            builder.RegisterType<UserStore<User, Role, int, UserExternLogin, UserRole, Claim>>().
                As<IUserStore<User, int>>().InstancePerRequest();
            builder.RegisterType<RoleStore<Role, int, UserRole>>().As<IRoleStore<Role, int>>().InstancePerRequest();

            builder.RegisterType<UserManager>().AsSelf().InstancePerRequest();
            builder.RegisterType<RoleManager>().AsSelf().InstancePerRequest();
            builder.RegisterType<SignInManager>().AsSelf().InstancePerRequest();
       

            builder.RegisterType<UnitOfWorkIdentity>().As<IUnitOfWorkIdentity>().InstancePerRequest();

            builder.Register<IDataProtectionProvider>(c => _app.GetDataProtectionProvider()).InstancePerRequest();
            builder.Register<IAuthenticationManager>(c => HttpContext.Current.GetOwinContext().Authentication).InstancePerRequest();
        }
    }
}