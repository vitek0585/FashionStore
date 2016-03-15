using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FashionStore.Infrastructure.Data.Identity.Entities;
using FashionStore.Infrastructure.Data.Identity.Resource;
using Microsoft.AspNet.Identity;

namespace FashionStore.Infrastructure.Data.Identity.Validation
{
    
    public class CustomUserValidation : UserValidator<User, int>
    {
        private UserManager<User, int> _manager;
 
        public bool RequireUniqueUserName { get; set; }

        public CustomUserValidation(UserManager<User, int> manager)
            : base(manager)
        {
            
            _manager = manager;
        }

        public override async Task<IdentityResult> ValidateAsync(User item)
        {
            if (item == null)
            {
                throw new ArgumentNullException("User is null");
            }
            var errors = new List<string>();
            if (RequireUniqueUserName)
            {
                await ValidateUserName(item, errors);
            }
            if (RequireUniqueEmail)
            {
                await ValidateEmail(item, errors);
            }
            if (errors.Any())
            {
                return new IdentityResult(errors);
            }
            return IdentityResult.Success;
        }

        private async Task ValidateUserName(User user, List<string> errors)
        {
            if (string.IsNullOrWhiteSpace(user.UserName))
            {
                errors.Add(IdentityMessages.NameInValid);
            }
            else
            {
                var owner = await _manager.FindByNameAsync(user.UserName);
                if (owner != null && owner.Id != user.Id)
                {
                    errors.Add(IdentityMessages.NameDuplicat);
                }
            }
        }

        private async Task ValidateEmail(User user, List<string> errors)
        {

            if (string.IsNullOrWhiteSpace(user.Email))
            {
                errors.Add(IdentityMessages.EmailInValid);
                return;
            }

            var owner = await _manager.FindByEmailAsync(user.Email);
            if (owner != null && owner.Id != user.Id)
            {
                errors.Add(IdentityMessages.EmailDuplicat);
            }

        }
    }
}