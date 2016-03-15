using System;
using System.Data;
using System.Threading.Tasks;

namespace FashionStore.Service.Interfaces.UoW
{
    public interface IUnitOfWork : IDisposable
    {
        void StartTransaction(IsolationLevel level = IsolationLevel.RepeatableRead);
        void Commit();
        void Rollback();
        void Save();
        Task<int> SaveAsync();
    }
}