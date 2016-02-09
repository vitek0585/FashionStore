using System.Collections;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using FashionStore.Controllers.Controller;
using FashionStore.Domain.Core.Entities.Store;
using FashionStore.Infrastructure.Data.Context.Store.Context;
using FashionStore.Models.Interfaces;
using FashionStore.Service.Interfaces.Services;
using FashionStore.Test.PresentationLayer.Controllers.Common;
using FashionStore.WorkFlow.ViewedStorage;
using FashionStore.WorkFlow.ViewedStorage.Provider.Interface;
using Moq;
using NUnit.Framework;
using TestStack.FluentMVCTesting;

namespace FashionStore.Test.PresentationLayer.Controllers
{
    [TestFixture]
    public class GoodCtrlTest : BaseCtrlTest
    {
        private Mock<IGoodService> _goodService;
        private Mock<IRecentlyViewedProvider> _viewedProvider;
        private GoodController _goodCtrl;
        [TestFixtureSetUp]
        public new void InitMock()
        {
            base.InitMock();
            _goodService = new Mock<IGoodService>();
            _viewedProvider = new Mock<IRecentlyViewedProvider>();
            _viewedProvider.Setup(v => v.TryGet(It.IsAny<string>(), It.IsAny<short>())).Returns(new RecentlyViewedStorage(10));

            _goodCtrl = new GoodController(_goodService.Object, _cookie.Object, _viewedProvider.Object);
            _goodCtrl.ControllerContext = CreateControllerContextFake(_goodCtrl);
        }
        [Test]
        public void GetDetailsTest()
        {
            var fake = GetFakeData<Good, ShopContext>(g => g.Goods);
            _goodService.Setup(s => s.GetGood<IGoodModel>(It.IsInRange(0, 100, Range.Exclusive), It.IsAny<string>(),
                It.IsAny<string>())).Returns((int i, string l, string c) =>
                    MapperType<Good, IGoodModel>(fake.FirstOrDefault(g => g.GoodId == i)));

            _goodCtrl.WithCallTo(c => c.GetDetails(1)).ShouldRenderView("Details").WithModel<IGoodModel>(m =>
            {
                Assert.That(m, Has.Property(GetNameData<Good>(g => g.GoodId)).EqualTo(1));
            });
        }
        [Test]
        public void RecentlyViewedUserTest()
        {
            var fake = GetFakeData<Good, ShopContext>(g => g.Goods);
            var storage = new RecentlyViewedStorage(10);
            for (var recentlyViewedId = 5; recentlyViewedId < 10; recentlyViewedId++)
            {
                storage.Add(recentlyViewedId);
            }
            _viewedProvider.Setup(v => v.TryGet(It.IsAny<string>(), It.IsAny<short>())).Returns(storage);

            _goodService.Setup(s => s.GetGoods<IGoodModel>(It.IsAny<IEnumerable<int>>(), It.IsAny<string>()))
                .Returns((IEnumerable<int> i, string l) =>
                    MapperCollection<Good, IGoodModel>(fake.Where(g => i.Contains(g.GoodId))));

            _goodCtrl.WithCallToChild(c => c.RecentlyViewedUser()).ShouldRenderPartialView("Widgets/_RecentlyViewed").
                WithModel<IEnumerable<IGoodModel>>(m =>
            {
                Assert.That(m.Count(), Is.EqualTo(5));
                Assert.That(m, Has.All.Matches<IGoodModel>(g => storage.GetAll().Contains(g.GoodId)));

            });
        }



    }
}