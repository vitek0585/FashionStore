using System.Collections.Generic;
using System.Linq;
using FashionStore.Application.Cart.Interfaces;

namespace FashionStore.WorkFlow.Cart
{


    public class UserCart : ICart<UserOrder>
    {
        private ICollection<UserOrder> _goods;

        public UserCart()
        {
            _goods = new List<UserOrder>();

        }

        public void AddGood(UserOrder good)
        {
            var comparer = new ComparerUserOrder();
            var target = _goods.FirstOrDefault(g => comparer.Equals(g, good));

            if (target != null)
                target.CountGood += good.CountGood;
            else
            {
                _goods.Add(good);
            }

        }

        public IEnumerable<UserOrder> GetAll()
        {
            return _goods;
        }

        public bool Update(int id, UserOrder goods)
        {
            var target = _goods.SingleOrDefault(g => g.ClassificationId == id);
            if (target != null)
            {
                target.ClassificationId = goods.ClassificationId;
                target.CountGood = goods.CountGood;
                target.ColorId = goods.ColorId;
                target.SizeId = goods.SizeId;
                return true;
            }
            return false;
        }
        public bool Remove(int id)
        {
            var good = _goods.SingleOrDefault(i => i.ClassificationId == id);
            if (good != null)
            {
                
                return _goods.Remove(good);
            }
            return false;
        }

        public void Clear()
        {
            _goods.Clear();
        }

        #region implements IEqualityComparer
        public class ComparerUserOrder : IEqualityComparer<UserOrder>
        {
            public bool Equals(UserOrder x, UserOrder y)
            {
                return GetHashCode(x) == GetHashCode(y) && x.ColorId == y.ColorId && x.SizeId == y.SizeId;
            }

            public int GetHashCode(UserOrder obj)
            {
                var hash = obj.GoodId.GetHashCode();
                return (hash << 20) * 2 ^ hash;
            }
        }
        #endregion
    }

}