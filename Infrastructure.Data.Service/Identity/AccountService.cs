using System;
using System.Threading.Tasks;
using FashionStore.Infastructure.Data.Identity.Entities;
using FashionStore.Infastructure.Data.Identity.Interfaces.Service;
using FashionStore.Infastructure.Data.Identity.Manager;
using FashionStore.Infastructure.Data.Service.Identity.Common;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;

namespace FashionStore.Infastructure.Data.Service.Identity
{
    public class AccountService : AccountGlobalService, IAccountService
    {
        public AccountService(RoleManager roleManager, UserManager userManager, SignInManager singInManager,
            IAuthenticationManager authentication)
            : base(roleManager, userManager, singInManager, authentication)
        {
        }
        
        public Task<IdentityResult> CreateUserAsync(User user, string password)
        {
            return _userManager.CreateAsync(user, password);
        }

        public async Task SendConfirmationTokenToEmailAsync(int userId, Func<string, string, object, string, string> urlHelper, string protocol = "http")
        {
            string code = await _userManager.GenerateEmailConfirmationTokenAsync(userId);
            var callbackUrl = urlHelper("ConfirmEmail", "Account", new { userId = userId, code = code }, protocol);

            await _userManager.SendEmailAsync(userId, "Confirm your account",
                "Please confirm your account by clicking <a href=\"" + callbackUrl + "\">here</a>  in the my shop is Vitek prodaction");
        }

        public Task<IdentityResult> ConfirmEmailAsync(int userId, string code)
        {
            return _userManager.ConfirmEmailAsync(userId, code);
        }

        public async Task<IdentityResult> LoginAsync(string userName, string password, bool rememberMe = false, params string[] errors)
        {
         
            var user = await _userManager.FindAsync(userName, password);
            if (user == null)
            {
                return new IdentityResult(errors[0]);
            }
            if (!user.EmailConfirmed)
            {
                return new IdentityResult(errors[1]);
            }
            var ident = await _userManager.CreateIdentityAsync(user, DefaultAuthenticationTypes.ApplicationCookie);
            _authentication.SignIn(new AuthenticationProperties() { IsPersistent = rememberMe }, ident);

            return IdentityResult.Success;
        }

        public Task<ExternalLoginInfo> GetExternalLoginInfoAsync()
        {
            return _authentication.GetExternalLoginInfoAsync();
        }

        public Task<SignInStatus> ExternalSignInAsync(ExternalLoginInfo loginInfo, bool isPersistent = false)
        {
            return _singInManager.ExternalSignInAsync(loginInfo, isPersistent);
        }

        public async Task<IdentityResult> CreateExternalUserAsync(User user)
        {
            var info = await _authentication.GetExternalLoginInfoAsync();
            if (info == null)
            {
                return null;
            }

            var result = await _userManager.CreateAsync(user);
            if (result.Succeeded)
            {
                result = await _userManager.AddLoginAsync(user.Id, info.Login);
                if (result.Succeeded)
                {
                    await _singInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                }
            }
            return result;
        }

        public void SingOut()
        {
            _authentication.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
        }
    }
}