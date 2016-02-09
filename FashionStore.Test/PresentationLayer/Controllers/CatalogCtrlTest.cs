using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using FashionStore.Controllers.Controller;
using FashionStore.Domain.Core.Entities.Store;
using FashionStore.Infrastructure.Data.Context.Store.Context;
using FashionStore.Models.Interfaces;
using FashionStore.Service.Interfaces.Services;
using FashionStore.Test.PresentationLayer.Controllers.Common;
using Moq;
using NUnit.Framework;
using TestStack.FluentMVCTesting;

namespace FashionStore.Test.PresentationLayer.Controllers
{
    using ICatModel = ITypeCategoryModel<ICategoryModel>;
    [TestFixture]
    public class CatalogCtrlTest : BaseCtrlTest
    {
        private Mock<ICategoryService> _categoryService;
        private CatalogController _controller;
        [TestFixtureSetUp]
        public new void InitMock()
        {
            base.InitMock();
            _categoryService = new Mock<ICategoryService>();

            _controller = new CatalogController(_categoryService.Object, _cookie.Object);
            _controller.ControllerContext = CreateControllerContextFake(_controller);
        }
        [Test]
        public void CategoriesTest()
        {
            var fake = GetFakeData<Category, ShopContext>(c => c.Categories);
            _categoryService.Setup(c => c.GetCategoriesByType<ICatModel>(It.IsAny<string>(), It.IsAny<string>()))
                .Returns(() => MapperType<Category, ICatModel>(fake.First()));

            _controller.WithCallTo(c => c.Categories(String.Empty)).ShouldRenderDefaultView().WithModel<ICatModel>(m =>
            {
                Assert.That(m,Is.Not.Null);
            });
         
        }

        [Test]
        public void CategoriesNotFoundTest()
        {
            _categoryService.Setup(c => c.GetCategoriesByType<ICatModel>(It.IsAny<string>(), It.IsAny<string>()))
                .Returns(() => null);

            var result = _controller.WithCallTo(c => c.Categories(String.Empty)).ActionResult;
            result.ExecuteResult(_controller.ControllerContext);
            Assert.That(_controller.Response.StatusCode, Is.EqualTo((int)HttpStatusCode.NotFound));
        }
        [Test]
        public void CategoryTest()
        {
            var fake = GetFakeData<Category, ShopContext>(c => c.Categories);
            _categoryService.Setup(c => c.GetCategoryByCulture<ICategoryDescModel>(It.IsAny<string>(),It.IsAny<int>(), It.IsAny<string>()))
                .Returns((string type, int i,string d) => MapperType<Category,ICategoryDescModel>(fake.FirstOrDefault(c=>c.CategoryId==i)));

            _controller.WithCallTo(c => c.Category(String.Empty,5)).ShouldRenderDefaultView()
                .WithModel<ICategoryDescModel>(m=>
            {
                Assert.That(m, Is.Not.Null);
            });
        }
        [Test]
        public void CategoryNotFoundTest()
        {
            _categoryService.Setup(c => c.GetCategoryByCulture<ICategoryDescModel>(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<string>()))
                .Returns(()=>null);

            _controller.WithCallTo(c => c.Category(String.Empty, 5))
                .ActionResult.ExecuteResult(_controller.ControllerContext);
            Assert.That(_controller.Response.StatusCode, Is.EqualTo((int)HttpStatusCode.NotFound));
             
        }
    }
}