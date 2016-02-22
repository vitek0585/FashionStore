using System;
using System.Threading.Tasks;
using FashionStore.Infrastructure.Data.Identity.Entities;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;

namespace FashionStore.Infrastructure.Data.Identity.Interfaces.Service
{
    public interface IAccountService
    {
        Task<IdentityResult> CreateUserAsync(User user, string password);
        Task SendConfirmationTokenToEmailAsync(int userId, Func<string, string, object, string, string> urlHelper, string protocol = "http");
        Task<IdentityResult> ConfirmEmailAsync(int userId, string code);
        Task<IdentityResult> LoginAsync(string userName, string password, bool rememberMe = false, params string[] errors);
        Task<ExternalLoginInfo> GetExternalLoginInfoAsync();
        Task<SignInStatus> ExternalSignInAsync(ExternalLoginInfo loginInfo, bool isPersistent = false);

        Task<IdentityResult> CreateExternalUserAsync(User user);
        void SingOut();
    }
}