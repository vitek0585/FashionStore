using System;
using FashionStore.Infrastructure.Data.Identity.Entities;
using FashionStore.Infrastructure.Data.Identity.Manager;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.Google;
using Microsoft.Owin.Security.Twitter;
using Owin;

namespace FashionStore.Infrastructure.Data.Identity.StartUp
{
    public class StartUpApp
    {
        public void ConfigureAuth(IAppBuilder app, string pathToLogin)
        {

            app.UseCookieAuthentication(new CookieAuthenticationOptions()
            {
                
                AuthenticationType = DefaultAuthenticationTypes.ApplicationCookie,
                LoginPath = new PathString(pathToLogin),
                
                Provider = new CookieAuthenticationProvider()
                {
                    OnValidateIdentity = SecurityStampValidator.OnValidateIdentity<UserManager, User, int>(
                    TimeSpan.FromMinutes(30),
                    (m, u) => u.GenerateUserIdentityAsync(m),
                    c => c.GetUserId<int>())
                }
            });

            app.UseExternalSignInCookie(DefaultAuthenticationTypes.ExternalCookie);

            app.UseTwitterAuthentication(new TwitterAuthenticationOptions()
            {
                ConsumerKey = "BEo5YcRRFFFiNCLnCrhrEFSbd",
                ConsumerSecret = "nGIZrR068lTOMJsqx8d6r9VnCjdmEVgJUKndHMpPdVfeCH1WFD",
                BackchannelCertificateValidator = new CertificateSubjectKeyIdentifierValidator(new[]
                {
                    "5168ff90af0207753cccd9656462a212b859723b"
                })
            });
            app.UseGoogleAuthentication(new GoogleOAuth2AuthenticationOptions()
            {
                ClientId = "812981170849-8fitj2cft62pc80thv4tu6b133nq0alg.apps.googleusercontent.com",
                ClientSecret = "BS2TvigoLB3byrn8-Q_o8n96"
            });

        }
    }
}