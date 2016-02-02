using System.Threading.Tasks;
using FashionStore.Infastructure.Data.Identity.Results;

namespace FashionStore.Infastructure.Data.Identity.Interfaces.Service
{
    public interface IUserIdentityService
    {
        Task<UserInfoIdentity> GetUserInfo(int id);
        int GetUserId();
    }
}