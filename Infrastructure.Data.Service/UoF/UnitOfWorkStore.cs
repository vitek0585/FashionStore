using System.Data;
using System.Data.Entity;
using FashionStore.Infrastructure.Data.Context.Store.Context;
using FashionStore.Service.Interfaces.UoW;

namespace FashionStore.Infastructure.Data.Service.UoF
{
    public class UnitOfWorkStore : IUnitOfWork
    {
        private DbContext _context;
        private DbContextTransaction _transaction;
        public UnitOfWorkStore(ShopContext context)
        {
            _context = context;
        }

        public void StartTransaction(IsolationLevel level)
        {
            _transaction = _context.Database.BeginTransaction(level);
        }

        public void Commit()
        {
            _transaction.Commit();
        }

        public void Rollback()
        {
            _transaction.Rollback();
        }

        public void Save()
        {
            _context.SaveChanges();
        }

        public void Dispose()
        {
            if (_transaction != null)
                _transaction.Dispose();
        }
    }
}