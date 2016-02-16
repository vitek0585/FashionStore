using Microsoft.AspNet.Identity.EntityFramework;

namespace FashionStore.Infastructure.Data.Identity.Entities
{
    public class UserRole:IdentityUserRole<int>
    {
        public virtual Role Role { get; set; }
        public virtual User User { get; set; }
    }
}