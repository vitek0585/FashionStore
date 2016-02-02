﻿using System;
using FashionStore.Infastructure.Data.Identity.Entities;
using FashionStore.Infastructure.Data.Identity.Results;
using FashionStore.Infastructure.Data.Identity.Utils;
using FashionStore.Infastructure.Data.Identity.Validation;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security.DataProtection;

namespace FashionStore.Infastructure.Data.Identity.Manager
{
    
    public class UserManager : UserManager<User, int>
    {
        public UserManager(IUserStore<User, int> store, IDataProtectionProvider provider)
            : base(store)
        {
            
            //UserValidator = new UserValidator<User, int>(this)
            //{
            //    AllowOnlyAlphanumericUserNames = true,
            //    RequireUniqueEmail = true,

            //};
            UserValidator = new CustomUserValidation(this)
            {
                RequireUniqueEmail = true,
                RequireUniqueUserName = true
            };
            PasswordValidator = new PasswordValidator()
            {
                RequireDigit = false,
                RequiredLength = 5,
                RequireLowercase = false,
                RequireUppercase = false,
                RequireNonLetterOrDigit = false
            };

            EmailService = new MailService();

            if (provider != null)
            {
                UserTokenProvider =
                    new DataProtectorTokenProvider<User, int>(provider.Create("ASP.NET Identity"))
                    {
                        TokenLifespan = TimeSpan.FromHours(6)
                    };
            }
        }




    }


}