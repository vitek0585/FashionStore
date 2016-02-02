using FashionStore.Infastructure.Data.Identity.Manager;
using Microsoft.Owin.Security;

namespace FashionStore.Infastructure.Data.Service.Identity.Common
{
    public abstract class AccountGlobalService
    {
        protected RoleManager _roleManager;
        protected UserManager _userManager;
        protected IAuthenticationManager _authentication;
        protected SignInManager _singInManager;
        protected AccountGlobalService(RoleManager roleManager, UserManager userManager,SignInManager singInManager,
            IAuthenticationManager authentication)
        {
            _singInManager = singInManager;
            _roleManager = roleManager;
            _userManager = userManager;
            _authentication = authentication;
        }
    }
}