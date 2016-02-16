using System.Data;
using System.Data.Entity;
using System.Threading.Tasks;
using FashionStore.Infastructure.Data.Identity.Context;
using FashionStore.Service.Interfaces.UoW;

namespace FashionStore.Infastructure.Data.Service.UoF
{
    public class UnitOfWorkIdentity : IUnitOfWorkIdentity
    {
        private DbContext _context;
        private DbContextTransaction _transaction;
        public UnitOfWorkIdentity(DbContextIdentity context)
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