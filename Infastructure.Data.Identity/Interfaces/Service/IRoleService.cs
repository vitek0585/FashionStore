using System.Collections.Generic;
using FashionStore.Infastructure.Data.Identity.Entities;
using FashionStore.Service.Interfaces.Services.Common;

namespace FashionStore.Infastructure.Data.Identity.Interfaces.Service
{
    public interface IRoleService
    {
        IEnumerable<Role> All();
    }
}