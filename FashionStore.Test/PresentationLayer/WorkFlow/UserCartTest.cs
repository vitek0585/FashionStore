using System.Linq;
using FashionStore.Models.Order;
using FashionStore.WorkFlow.Cart;
using NUnit.Framework;

namespace FashionStore.Test.PresentationLayer.WorkFlow
{
    [TestFixture]
    public class UserCartTest
    {
        readonly UserCart _userCart = new UserCart();
        [Test]
        public void AddTest()
        {
            var order = new UserOrderModel()
            {
                GoodId = 2,
                ColorId = 1,
                SizeId = 2,
                CountGood = 1
            };
            var order1 = new UserOrderModel()
            {
                GoodId = 2,
                ColorId = 2,
                SizeId = 2,
                CountGood = 2
            };
            _userCart.AddGood(order);
            _userCart.AddGood(order);
            _userCart.AddGood(order1);

            Assert.AreEqual(_userCart.GetAll().Count(), 2);
            Assert.AreEqual(_userCart.GetAll().Sum(o => o.CountGood), 4);
            Assert.AreEqual(_userCart.GetAll().First().CountGood, 2);
            Assert.AreEqual(_userCart.GetAll().Last().CountGood, 2);
        }
        [Test]
        public void RemoveTest()
        {
            var order = new UserOrderModel()
            {
                GoodId = 2,
                ColorId = 1,
                SizeId = 2,
                CountGood = 1,
                ClassificationId = 1
            };
            var order1 = new UserOrderModel()
            {
                GoodId = 3,
                ColorId = 2,
                SizeId = 2,
                CountGood = 2,
                ClassificationId = 2
            };
            _userCart.AddGood(order);
            _userCart.AddGood(order);
            _userCart.AddGood(order1);

            _userCart.Remove(order1.ClassificationId);
            Assert.AreEqual(_userCart.GetAll().Count(), 1);
      
        }
        [Test]
        public void NotRemoveTest()
        {
            var order = new UserOrderModel()
            {
                GoodId = 2,
                ColorId = 1,
                SizeId = 2,
                CountGood = 1,
                ClassificationId = 1
            };
            var order1 = new UserOrderModel()
            {
                GoodId = 2,
                ColorId = 2,
                SizeId = 2,
                CountGood = 2,
                ClassificationId = 2

            };
            _userCart.AddGood(order);
            _userCart.AddGood(order);
            _userCart.AddGood(order1);

            order1 = new UserOrderModel()
            {
                GoodId = 2,
                ColorId = 2,
                SizeId = 10,
                CountGood = 3
            }; 
            Assert.False(_userCart.Remove(order1.ClassificationId));
            Assert.AreEqual(2,_userCart.GetAll().Count());

        }
    }
}