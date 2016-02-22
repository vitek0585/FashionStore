using System.Security.Claims;
using System.Threading.Tasks;
using FashionStore.Infrastructure.Data.Identity.Entities;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;

namespace FashionStore.Infrastructure.Data.Identity.Manager
{
    public class SignInManager:SignInManager<User,int>
    {
        public SignInManager(UserManager userManager, IAuthenticationManager authenticationManager) : 
            base(userManager, authenticationManager)
        {
            
        }

        public override Task<ClaimsIdentity> CreateUserIdentityAsync(User user)
        {
            return user.GenerateUserIdentityAsync((UserManager)UserManager);
        }

    }
}