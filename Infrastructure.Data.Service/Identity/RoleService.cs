using System.Collections.Generic;
using FashionStore.Infastructure.Data.Identity.Entities;
using FashionStore.Infastructure.Data.Identity.Interfaces.Service;
using FashionStore.Infastructure.Data.Identity.Manager;

namespace FashionStore.Infastructure.Data.Service.Identity
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