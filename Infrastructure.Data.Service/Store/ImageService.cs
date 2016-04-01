using System;
using System.Data;
using System.IO;
using System.Threading.Tasks;
using FashionStore.Domain.Core.Entities.Store;
using FashionStore.Domain.Interfaces.Repository;
using FashionStore.Infrastructure.Data.Service.Store.Common;
using FashionStore.Service.Interfaces.Services;
using FashionStore.Service.Interfaces.UoW;

namespace FashionStore.Infrastructure.Data.Service.Store
{
    public class ImageService : EntityService<Image>, IImageService
    {
        private IImageRepository _repository;
        public ImageService(IUnitOfWorkStore unitOfWork, IImageRepository repository)
            : base(unitOfWork, repository)
        {
            _repository = repository;

        }

        public async Task<Image> AddImageAsync(int id, byte[] image, string path)
        {
            if (File.Exists(path))
                throw new ArgumentException("The file name already exists");
            try
            {
                _unitOfWork.StartTransaction(IsolationLevel.ReadCommitted);
                var img = new Image()
                {
                    GoodId = id,
                    ImagePath = Path.GetFileName(path)
                };
                var isAdded = _repository.AddWithoutId(img);
                if (!isAdded)
                    throw new ArgumentException("The image have not added");
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