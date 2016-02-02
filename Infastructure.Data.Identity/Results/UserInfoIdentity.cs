using System.Collections.Generic;
using FashionStore.Infastructure.Data.Identity.Entities;

namespace FashionStore.Infastructure.Data.Identity.Results
{
    public class UserInfoIdentity
    {
        public bool HasPassword { get; set; }
        public string PhoneNumber { get; set; }
        public ICollection<UserExternLogin> Logins { get; set; }
  
        public string UserName { get; set; }
        public string Email { get; set; }
    }
}