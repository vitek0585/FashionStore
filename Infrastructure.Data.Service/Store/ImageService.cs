using System;
using System.Data;
using System.IO;
using System.Threading.Tasks;
using FashionStore.Domain.Core.Entities.Store;
using FashionStore.Domain.Interfaces.Repository;
using FashionStore.Domain.Interfaces.Repository.Common;
using FashionStore.Infastructure.Data.Service.Store.Common;
using FashionStore.Service.Interfaces.Services;
using FashionStore.Service.Interfaces.UoW;

namespace FashionStore.Infastructure.Data.Service.Store
{
    public class ImageService : EntityService<Image>, IImageService
    {

        public ImageService(IUnitOfWorkStore unitOfWork, IImageRepository repository)
            : base(unitOfWork, repository)
        {


        }

        public async Task<Image> AddImage(int id, byte[] image, string path)
        {
            if (File.Exists(path))
                throw new ArgumentException("The file name already exists");
            try
            {
                _unitOfWork.StartTransaction(IsolationLevel.ReadUncommitted);
                var img = _repository.Add(new Image()
                {
                    GoodId = id,
                    ImagePath = Path.GetFileName(path)
                });
                await _unitOfWork.SaveAsync();
                using (var fs = File.Create(path))
                {
                    await fs.WriteAsync(image, 0, image.Length);
                }
                _unitOfWork.Commit();
                return img;
            }
            catch (Exception e)
            {
                _unitOfWork.Rollback();
                throw;
            }
        }

    }
}