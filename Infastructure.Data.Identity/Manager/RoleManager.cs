using FashionStore.Infastructure.Data.Identity.Context;
using FashionStore.Infastructure.Data.Identity.Entities;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;

namespace FashionStore.Infastructure.Data.Identity.Manager
{
    public class RoleManager:RoleManager<Role,int>
    {
        public RoleManager(IRoleStore<Role, int> store) : base(store)
        {

        }

        public static RoleManager Create(IdentityFactoryOptions<RoleManager> options,IOwinContext context)
        {
            var roleStore = new RoleStore<Role,int,UserRole>(context.Get<DbContextIdentity>());
            return new RoleManager(roleStore);
        }
    }
}