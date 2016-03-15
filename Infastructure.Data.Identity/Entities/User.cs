using System.Security.Claims;
using System.Threading.Tasks;
using FashionStore.Infrastructure.Data.Identity.Manager;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace FashionStore.Infrastructure.Data.Identity.Entities
{
    public class User:IdentityUser<int,UserExternLogin,UserRole,Claim>
    {
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager userManager)
        {
            var userIdentity = await userManager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);

            return userIdentity;
        }
    }
}