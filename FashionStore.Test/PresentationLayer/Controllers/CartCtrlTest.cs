using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Web;
using FashionStore.Configuration.Mapper;
using FashionStore.Controllers.Controller;
using FashionStore.Domain.Core.Entities.Store;
using FashionStore.Infrastructure.Data.Context.Store.Context;
using FashionStore.Infrastructure.Data.Identity.Interfaces.Service;
using FashionStore.Models.Order;
using FashionStore.Service.Interfaces.Results;
using FashionStore.Service.Interfaces.Services;
using FashionStore.Test.PresentationLayer.Controllers.Common;
using FashionStore.WorkFlow.Cart.Interfaces;
using FashionStore.WorkFlow.Cart.Interfaces.Provider;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;
using Moq;
using NUnit.Framework;
using TestStack.FluentMVCTesting;
using WebCookie.Interfaces;
using WebLogger.Abstract.Interface;

namespace FashionStore.Test.PresentationLayer.Controllers
{
    [TestFixture]
    public class CartCtrlTest : BaseCtrlTest
    {
       
        Mock<ICartProvider<UserOrderModel>> _cartProvider = new Mock<ICartProvider<UserOrderModel>>();
        Mock<IPurchaseService> _purchaseService = new Mock<IPurchaseService>();
        Mock<ICart<UserOrderModel>> _cart = new Mock<ICart<UserOrderModel>>();
        Mock<IUserAppService> _auth = new Mock<IUserAppService>();

        private CartController _ctrl;

        [TestFixtureSetUp]
        public new void InitMock()
        {
            base.InitMock();

            _cartProvider.Setup(m => m.GetCart()).Returns(_cart.Object);

            _ctrl = new CartController(_cookie.Object, _cartProvider.Object, _purchaseService.Object,_auth.Object,_log.Object);
            _ctrl.ControllerContext = CreateControllerContextFake(_ctrl);
           
        }
        [Test]
        public void ValidationModel()
        {
            var model = new UserOrderModel()
            {
                GoodId = 1,
                SizeId = 1,
                ColorId = 1
            };
           
            Assert.AreEqual(1, Validate(model).Count);
            Assert.That(Validate(model).SelectMany(v => v.MemberNames),
                Has.Some.EqualTo(GetNameData<UserOrderModel>(o => o.CountGood)));

            model = new UserOrderModel()
            {
                GoodId = 1,
                SizeId = 1,
                ColorId = 1,
                CountGood = 1
            };
            Assert.AreEqual(0, Validate(model).Count);
        }

        [Test]
        public void CartAction()
        {
            Expression<Func<ShopContext, object>> exp = c => c.ClassificationGoods;
            var data = GetFakeData<UserOrderModel, ShopContext>(exp);

            _purchaseService.Setup(m => m.GetGoodsByCart<UserOrderModel>(It.IsAny<IEnumerable<ClassificationGood>>(),
                It.IsAny<string>(), It.IsAny<string>()))
                .Returns(data);

            _ctrl.WithCallTo(c => c.Cart()).ShouldRenderDefaultView()
                .WithModel<IEnumerable<UserOrderModel>>(m =>
            {
                Assert.That(m, Has.Some.Matches<UserOrderModel>(o => o.GoodId == 1));

            }).AndNoModelErrors();

        }
        [Test]
        public void AddBadRequestAction()
        {
            var order = new UserOrderModel();

            _purchaseService.Setup(m => m.GetClassification(It.IsAny<ClassificationGood>())).Returns<ClassificationGood>(null);

            var result = _ctrl.WithCallTo(c => c.Add(order)).ShouldReturnJson();
            result.ExecuteResult(_ctrl.ControllerContext);


            _cartProvider.Verify(c => c.GetCart(), Times.Never);
            Assert.AreEqual((int)HttpStatusCode.BadRequest, _ctrl.ControllerContext.HttpContext.Response.StatusCode);
        }
        [Test]
        public void AddCorrectAction()
        {
            var order = new UserOrderModel();

            _purchaseService.Setup(m => m.GetClassification(It.IsAny<ClassificationGood>())).Returns(new ClassificationGood());

            var result = _ctrl.WithCallTo(c => c.Add(order)).ShouldReturnJson();
            result.ExecuteResult(_ctrl.ControllerContext);

            _cartProvider.Verify(c => c.GetCart(), Times.Once);
            Assert.AreEqual((int)HttpStatusCode.Created, _ctrl.ControllerContext.HttpContext.Response.StatusCode);
        }
        [Test]
        public void RemoveBadRequestAction()
        {

            _cart.Setup(m => m.Remove(It.IsAny<int>())).Returns(false);
            _cartProvider.Setup(m => m.GetCart()).Returns(_cart.Object);

            var res = _ctrl.WithCallTo(c => c.Remove(It.IsAny<int>())).ShouldReturnJson();
            res.ExecuteResult(_ctrl.ControllerContext);
            Assert.AreEqual((int)HttpStatusCode.BadRequest, _ctrl.ControllerContext.HttpContext.Response.StatusCode);

        }
        [Test]
        public void RemoveCorrectAction()
        {
            Mock<ICart<UserOrderModel>> cart = new Mock<ICart<UserOrderModel>>();
            cart.Setup(m => m.Remove(It.IsAny<int>())).Returns(true);
            _cartProvider.Setup(m => m.GetCart()).Returns(cart.Object);

            var res = _ctrl.WithCallTo(c => c.Remove(It.IsAny<int>())).ShouldReturnJson();
            res.ExecuteResult(_ctrl.ControllerContext);
            Assert.AreEqual((int)HttpStatusCode.OK, _ctrl.ControllerContext.HttpContext.Response.StatusCode);
        }

        [Test]
        public void DoOrderCorrectAuhtorizeAction()
        {
           _cart.Setup(c => c.GetAll()).Returns(Enumerable.Empty<UserOrderModel>());
          
           _auth.Setup(c => c.GetUserId()).Returns(1);
           _purchaseService.Setup(
               p => p.MakeAnOrder(It.IsAny<IEnumerable<ClassificationGood>>(),It.IsAny<int>(), It.IsAny<string>(),
                   It.IsAny<string>(), It.IsAny<string>())).Returns(new PurchaseResult());

            var res = _ctrl.WithCallTo(c => c.DoOrder()).ShouldReturnJson();
            res.ExecuteResult(_ctrl.ControllerContext);

            Assert.AreEqual((int)HttpStatusCode.Accepted, _ctrl.ControllerContext.HttpContext.Response.StatusCode);
        }
        [Test]
        public void DoOrderBadRequestAction()
        {

            _cart.Setup(c => c.GetAll()).Returns(Enumerable.Empty<UserOrderModel>());
            _purchaseService.Setup(
                p => p.MakeAnOrder(It.IsAny<IEnumerable<ClassificationGood>>(), It.IsAny<int>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                .Returns(new PurchaseResult(string.Empty));

            var res = _ctrl.WithCallTo(c => c.DoOrder()).ShouldReturnJson();
            res.ExecuteResult(_ctrl.ControllerContext);


            Assert.AreEqual((int)HttpStatusCode.BadRequest, _ctrl.ControllerContext.HttpContext.Response.StatusCode);
        }

    }
}