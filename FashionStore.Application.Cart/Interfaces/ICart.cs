using System.Collections.Generic;

namespace FashionStore.Application.Cart.Interfaces
{
    public interface ICart<TItem>
    {
        void AddGood(TItem good);
        IEnumerable<TItem> GetAll();
        bool Update(int id,TItem goods);

        bool Remove(int id);

        void Clear();
    }
}