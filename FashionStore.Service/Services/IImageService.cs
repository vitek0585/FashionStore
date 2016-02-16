using System.Threading.Tasks;
using FashionStore.Domain.Core.Entities.Store;
using FashionStore.Service.Interfaces.Services.Common;

namespace FashionStore.Service.Interfaces.Services
{
    public interface IImageService : IEntityService<Image>
    {
        Task<Image> AddImage(int id, byte[] image, string path);
    }
}