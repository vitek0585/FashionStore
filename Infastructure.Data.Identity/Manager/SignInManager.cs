using System.Security.Claims;
using System.Threading.Tasks;
using FashionStore.Infastructure.Data.Identity.Entities;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;

namespace FashionStore.Infastructure.Data.Identity.Manager
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