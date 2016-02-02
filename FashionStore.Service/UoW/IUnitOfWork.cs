using System;
using System.Data;

namespace FashionStore.Service.Interfaces.UoW
{
    public interface IUnitOfWork : IDisposable
    {
        void StartTransaction(IsolationLevel level = IsolationLevel.RepeatableRead);
        void Commit();
        void Rollback();
        void Save();
    }
}