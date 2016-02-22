using System.Collections.Generic;
using FashionStore.Infrastructure.Data.Identity.Entities;
using FashionStore.Infrastructure.Data.Identity.Interfaces.Service;
using FashionStore.Infrastructure.Data.Identity.Manager;

namespace FashionStore.Infrastructure.Data.Service.Identity
{
    public class RoleService:IRoleService
    {
        private RoleManager _roleManager;

        public RoleService(RoleManager roleManager)
        {
            _roleManager = roleManager;
        }

        public IEnumerable<Role> All()
        {
            return _roleManager.Roles;
        }
    }
}