using FashionStore.Infrastructure.Data.Identity.Context;
using FashionStore.Infrastructure.Data.Identity.Entities;
using FashionStore.Infrastructure.Data.Repository.Common;

namespace FashionStore.Infrastructure.Data.Repository.Identity
{
    public class UserRepository : GlobalRepository<User>
    {
        public UserRepository(DbContextIdentity context)
            : base(context)
        {

        }
    }
}