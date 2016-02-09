using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using AutoMapper;
using FashionStore.App_GlobalResources;
using FashionStore.Controllers.Base;
using FashionStore.Core.Filter.ModelValidate;
using FashionStore.Domain.Core.Entities.Store;
using FashionStore.Infastructure.Data.Identity.Interfaces.Service;
using FashionStore.Models.Order;
using FashionStore.Service.Interfaces.Results;
using FashionStore.Service.Interfaces.Services;
using FashionStore.ViewModels.Account;
using FashionStore.WorkFlow.Cart.Interfaces;
using FashionStore.WorkFlow.Cart.Interfaces.Provider;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;
using WebCookie.Interfaces;
using WebLogger.Abstract.Interface;

namespace FashionStore.Controllers.Controller
{
    [RoutePrefix("Cart")]
    public class CartController : ShopBaseController
    {
        private IPurchaseService _purchaseService;
        private ICartProvider<UserOrderModel> _cartProvider;
        private ILogWriter<string> _log;
        private IUserAppService _user;
        public CartController(ICookieConsumer storage, ICartProvider<UserOrderModel> cartProvider, IPurchaseService purchaseService,
            IUserAppService user, ILogWriter<string> log)
            : base(storage)
        {
            _user = user;
            _log = log;
            _cartProvider = cartProvider;
            _purchaseService = purchaseService;
        }
        [Route("")]
        public ActionResult Cart()
        {
            var cart = GetCart();
            var orders = cart.GetAll();
            var data = _purchaseService.GetGoodsByCart<UserOrderModel>(MapToClassificationGoods(orders), GetCurrentCurrency(), GetCurrentLanguage());
            return View(data);
        }
        [HttpPost]
        [Route("Add")]
        [ValidateModelMvc]
        public JsonResult Add([Bind(Include = "GoodId,SizeId,ColorId,CountGood")]UserOrderModel order)
        {
            try
            {
               
                var item = _purchaseService.GetClassification(Mapper.Map<ClassificationGood>(order));
                if (item != null)
                {
                    order.ClassificationId = item.ClassificationId;
                    var cart = GetCart();
                    cart.AddGood(order);
                    return JsonResultCustom(Resource.AddToCartSuccess, HttpStatusCode.Created);
                }
            }
            catch (Exception e)
            {
                _log.LogWriteError("erorr add cart",e);
                return JsonResultCustom(e.Message, HttpStatusCode.BadRequest);

            }
            return JsonResultCustom(Resource.AddToCartError, HttpStatusCode.BadRequest);

        }
        
        [Route("DoOrder")]
        [ValidateModelMvc]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult DoOrder(QuickOrderViewModel user)
        {
            PurchaseResult result;
            try
            {
                result = Ordering(null,user.UserName,user.Email,user.PhoneNumber);
                if (result.Success)
                    return JsonResultCustom(Resource.BuySuccess, HttpStatusCode.Accepted);
            }
            catch (Exception e)
            {
                return JsonResultCustom(e.Message, HttpStatusCode.BadRequest);
            }
            if (result.Exception != null)
            {
                _log.LogWriteError("erorr DoOrder cart", result.Exception);
            }
            return JsonResultCustom(result.Error, HttpStatusCode.BadRequest);
        }
        [Route("DoOrderReg")]
        [HttpPost]
        [Authorize]
        public JsonResult DoOrder()
        {
            PurchaseResult result;
            try
            {
                var id = GetUserId();
                result = Ordering(id);
                if (result.Success)
                    return JsonResultCustom(Resource.BuySuccess, HttpStatusCode.Accepted);
            }
            catch (Exception e)
            {
                return JsonResultCustom(Resource.AnotherError, HttpStatusCode.BadRequest);
            }
            if (result.Exception != null)
            {
                _log.LogWriteError("erorr DoOrderReg cart", result.Exception);
            }
            return JsonResultCustom(result.Error, HttpStatusCode.BadRequest);
        }
        [Route("Remove")]
        [HttpPost]
        [ValidateModelMvc]
        public JsonResult Remove(int id)
        {
            try
            {
                var cart = GetCart();
                var isDrop = cart.Remove(id);

                if (isDrop)
                    return JsonResultCustom(Resource.Success);
            }
            catch (Exception e)
            {
                _log.LogWriteError("erorr Remove cart", e);
            }
            return JsonResultCustom(Resource.AnotherError, HttpStatusCode.BadRequest);

        }
        [Route("GetCart")]
        public JsonResult CartGoods()
        {
            var cart = GetCart();
            return JsonResultCustom(cart.GetAll());

        }
        [Route("Details")]
        public JsonResult GetDetailsGoods(int id)
        {
            var data = _purchaseService.GetGoodsDetails<dynamic>(id);

            if (data.Any())
                return JsonResultCustom(data);

            return JsonResultCustom(Resource.AnotherError, HttpStatusCode.BadRequest);
        }
        [Route("Update")]
        public JsonResult UpdateCart([Bind(Include = "ClassificationId,GoodId,SizeId,ColorId,CountGood")]UserOrderModel order)
        {
            var item = _purchaseService.GetClassification(Mapper.Map<ClassificationGood>(order));
            if (item != null)
            {
                var cart = GetCart();
                var idToReplace = order.ClassificationId;
                order.ClassificationId = item.ClassificationId;
                var isUpdate = cart.Update(idToReplace, order);
                if (isUpdate)
                    return JsonResultCustom(String.Empty);
            }

            return JsonResultCustom(Resource.AnotherError, HttpStatusCode.BadRequest);
        }
       
        #region Helpers method

        [NonAction]
        private int GetUserId()
        {
            return _user.GetUserId(); 

        }
        [NonAction]
        private ICart<UserOrderModel> GetCart()
        {
            return _cartProvider.GetCart();
        }

        [NonAction]
        private PurchaseResult Ordering(int? userId, string userName = null, string phone = null, string email = null)
        {
            var cart = GetCart();
            var orders = cart.GetAll();
            var result = _purchaseService.MakeAnOrder(MapToClassificationGoods(orders),userId, 
                userName, phone, email);
            if (result.Success)
            {
                cart.Clear();
            }
            return result;

        }

        [NonAction]
        private static IEnumerable<ClassificationGood> MapToClassificationGoods(IEnumerable<UserOrderModel> orders)
        {
            return Mapper.Map<IEnumerable<ClassificationGood>>(orders);
        }

        #endregion

    }


}