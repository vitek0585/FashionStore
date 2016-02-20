using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using FashionStore.Controllers.WebApi;
using FashionStore.Core.WebApiFormatters.Interfaces;
using FashionStore.Infastructure.Data.Repository.Store;
using FashionStore.Infastructure.Data.Service.Store;
using FashionStore.Infastructure.Data.Service.UoF;
using FashionStore.Infrastructure.Data.Context.Store.Context;
using FashionStore.Test.PresentationLayer.Controllers.Common;
using NUnit.Framework;
using WebLogger.WebLog;

namespace FashionStore.Test.PresentationLayer.Controllers
{

    [TestFixture]
    public class PhotoCtrlTest : BaseCtrlTest
    {
        private PhotoController _ctrl;
        [TestFixtureSetUp]
        public new void InitMock()
        {
            base.InitMock();


        }
        [Test]
        public async void AddImageWithRealContextTest()
        {

            var count = 5;
            var contexts = new List<ShopContext>();
            for (int i = 0; i < count; i++)
            {
                contexts.Add(new ShopContext());
            }
            var uof = new List<UnitOfWorkStore>();
            for (int i = 0; i < count; i++)
            {
                uof.Add(new UnitOfWorkStore(contexts.ElementAt(i)));
            }
            var repo = new List<ImageRepository>();
            for (int i = 0; i < count; i++)
            {
                repo.Add(new ImageRepository(contexts.ElementAt(i)));
            }
            var imgSvc = new List<ImageService>();
            for (int i = 0; i < count; i++)
            {
                imgSvc.Add(new ImageService(uof.ElementAt(i), repo.ElementAt(i)));
            }
            var ctrls = new List<PhotoController>();
            for (int i = 0; i < count; i++)
            {
                ctrls.Add(new PhotoController(uof.ElementAt(i), imgSvc.ElementAt(i), _log.Object)
                {
                    Request = new HttpRequestMessage()
                });
            }
            for (int i = 0; i < count; i++)
            {
                await ctrls.ElementAt(i).AddPhoto(521,new FileData()
                {
                    FileName = "test"+i,
                });
            }
        }


    }
}