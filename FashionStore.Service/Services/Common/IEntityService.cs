namespace FashionStore.Service.Interfaces.Services.Common
{
    public interface IEntityService<T>
    {
        T Add(T item);
        void Delete(T item);
        void Save();
        void Update(T item); 
    }
}