using FashionStore.Infastructure.Data.Identity.Context;
using FashionStore.Infastructure.Data.Identity.Entities;
using FashionStore.Infastructure.Data.Repository.Common;

namespace FashionStore.Infastructure.Data.Repository.Identity
{
    public class UserRepository : GlobalRepository<User>
    {
        public UserRepository(DbContextIdentity context)
            : base(context)
        {

        }
    }
}