using System;
using System.IO;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using FashionStore.Controllers.Controller;
using FashionStore.Models.Interfaces;
using FashionStore.Service.Interfaces.Services;
using FashionStore.Test.PresentationLayer.Controllers.Common;
using Moq;
using NUnit.Framework;
using TestStack.FluentMVCTesting;

namespace FashionStore.Test.PresentationLayer.Controllers
{
    [TestFixture]
    public class MainCtrlTest : BaseCtrlTest
    {
        private Mock<ICategoryService> _categoryService;
        private MainController _controller;
        [TestFixtureSetUp]
        public new void InitMock()
        {
            _categoryService = new Mock<ICategoryService>();
            _controller = new MainController(_categoryService.Object, _cookie.Object);

            _controller.ControllerContext = CreateControllerContextFake(_controller);
        }
        [Test]
        public void IndexTest()
        {
            _categoryService.Setup(c => c.GetRandom<ICategoryModel>(It.IsAny<int>(), It.IsAny<string>()))
                .Returns(() => null);
            _controller.WithCallTo(c => c.Index()).ShouldRenderDefaultView();
        }
        [Test]
        public void ChangeLanguageTest()
        {
            _cookie.Setup(c => c.SetValueStorage(It.IsAny<HttpContextBase>(), It.IsAny<string>(),
                It.IsAny<string>(), It.IsAny<string[]>()));
            
            var context = new HttpContext(new HttpRequest(String.Empty, "http://localhost", String.Empty),
                new HttpResponse(TextWriter.Null));
           
            RouteCollection routeCollection = new RouteCollection();
            routeCollection.MapRoute("Main", "{controller}/{action}",
               new { action = "Index", controller = "Main" });
            
            _controller.Url = new UrlHelper(new RequestContext(new HttpContextWrapper(context), new RouteData()),
                routeCollection);

            _controller.WithCallTo(c => c.ChangeLanguage(String.Empty, "http://localhost/Catalog/Men"))
                .ShouldRedirectTo("http://localhost/Catalog/Men");

            _controller.WithCallTo(c => c.ChangeLanguage(String.Empty, String.Empty))
                .ShouldRedirectTo(_controller.Url.Action("Index", "Main"));
        }
        [Test]
        public void ChangeCurrencyTest()
        {
            var context = new HttpContext(new HttpRequest(String.Empty, "http://localhost", String.Empty),
                new HttpResponse(TextWriter.Null));

            RouteCollection routeCollection = new RouteCollection();
            routeCollection.MapRoute("Main", "{controller}/{action}",
               new { action = "Index", controller = "Main" });

            _controller.Url = new UrlHelper(new RequestContext(new HttpContextWrapper(context), new RouteData()),
                routeCollection);

            _controller.WithCallTo(c => c.ChangeCurrency(String.Empty, "http://localhost/Catalog/Men"))
                .ShouldRedirectTo("http://localhost/Catalog/Men");

            _controller.WithCallTo(c => c.ChangeLanguage(String.Empty, String.Empty))
                .ShouldRedirectTo(_controller.Url.Action("Index", "Main"));
        }
    }
}