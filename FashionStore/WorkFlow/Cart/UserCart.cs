using System.Collections.Generic;
using System.Linq;
using FashionStore.Models.Order;
using FashionStore.WorkFlow.Cart.Interfaces;

namespace FashionStore.WorkFlow.Cart
{


    public class UserCart : ICart<UserOrderModel>
    {
        private ICollection<UserOrderModel> _goods;

        public UserCart()
        {
            _goods = new List<UserOrderModel>();

        }

        public void AddGood(UserOrderModel good)
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

        public IEnumerable<UserOrderModel> GetAll()
        {
            return _goods;
        }

        public bool Update(int id, UserOrderModel goods)
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
        public class ComparerUserOrder : IEqualityComparer<UserOrderModel>
        {
            public bool Equals(UserOrderModel x, UserOrderModel y)
            {
                return GetHashCode(x) == GetHashCode(y) && x.ColorId == y.ColorId && x.SizeId == y.SizeId;
            }

            public int GetHashCode(UserOrderModel obj)
            {
                var hash = obj.GoodId.GetHashCode();
                return (hash << 20) * 2 ^ hash;
            }
        }
        #endregion
    }

}