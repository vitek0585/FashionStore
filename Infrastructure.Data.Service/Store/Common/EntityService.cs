using System.Collections.Generic;
using AutoMapper;
using FashionStore.Domain.Interfaces.Repository.Common;
using FashionStore.Service.Interfaces.Services.Common;
using FashionStore.Service.Interfaces.UoW;

namespace FashionStore.Infrastructure.Data.Service.Store.Common
{
    public abstract class EntityService<T> : EntityServiceBase, IEntityService<T> where T : class
    {
        protected IUnitOfWork _unitOfWork;

        protected IGlobalRepository<T> _repository;
        public EntityService(IUnitOfWork unitOfWork, IGlobalRepository<T> repository)
        {
            _unitOfWork = unitOfWork;
            _repository = repository;
        }

        public T Add(T item)
        {
            return _repository.Add(item);
        }

        public void Delete(T item)
        {
            _repository.Delete(item);
        }

        public void Save()
        {
            _unitOfWork.Save();
        }

        public void Update(T item)
        {
            _repository.Update(item);
        }

        public IEnumerable<T> All()
        {
            return _repository.GetAll();
        }

        protected IEnumerable<TResult> MapCollection<TResult>(dynamic data)
        {
            return Mapper.DynamicMap<IEnumerable<TResult>>(data);
        }
    }
}