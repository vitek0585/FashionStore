using System.Data;
using System.Data.Entity;
using System.Threading.Tasks;
using FashionStore.Infrastructure.Data.Context.Store.Context;
using FashionStore.Service.Interfaces.UoW;

namespace FashionStore.Infrastructure.Data.Service.UoF
{
    public class UnitOfWorkStore : IUnitOfWorkStore
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
        public Task<int> SaveAsync()
        {
            return _context.SaveChangesAsync();
        }
        public void Dispose()
        {
            if (_transaction != null)
                _transaction.Dispose();
        }
    }
}