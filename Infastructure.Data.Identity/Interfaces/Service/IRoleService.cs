using System.Collections.Generic;
using FashionStore.Infrastructure.Data.Identity.Entities;

namespace FashionStore.Infrastructure.Data.Identity.Interfaces.Service
{
    public interface IRoleService
    {
        IEnumerable<Role> All();
    }
}