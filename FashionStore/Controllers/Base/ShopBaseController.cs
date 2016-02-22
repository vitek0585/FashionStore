using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using System.Web.Mvc;
using FashionStore.Core.AppValue;
using FashionStore.Utils.CustomResult;
using WebCookie.Interfaces;

namespace FashionStore.Controllers.Base
{
    public abstract class ShopBaseController : System.Web.Mvc.Controller
    {
        protected ICookieConsumer _storage;

        protected ShopBaseController(ICookieConsumer storage)
        {
            _storage = storage;
        }

        #region Helper methods
        [NonAction]
        protected string CheckValidReturnUrl(string returnUrl)
        {
            if (!string.IsNullOrEmpty(returnUrl))
            {
                Uri uri = new Uri(returnUrl);
                if (Url.IsLocalUrl(uri.AbsolutePath))
                    return returnUrl;
            }
            return Url.Action("Index", "Main");
        }
      
        [NonAction]
        protected string GetCurrentCurrency()
        {
            return _storage.GetValueStorage(HttpContext, ValuesApp.Currency)
                   ?? ValuesApp.CurrencyDefault;
        }
        [NonAction]
        protected string GetCurrentLanguage()
        {
            return Thread.CurrentThread.CurrentCulture.TwoLetterISOLanguageName;
        }
        [NonAction]
        protected IEnumerable<string> ReturnErrorModelState()
        {
            return ModelState.Values.SelectMany(e => e.Errors, (m, e) => e.ErrorMessage).ToArray();
        }

        #endregion
        #region custom result
        protected JsonResult JsonResultCustom(object data, HttpStatusCode statusCode)
        {
            return new JsonResultCustom(data, statusCode);
        }
        
        protected JsonResult JsonResultCustom(object data)
        {
            return new JsonResultCustom(data);
        }
        protected JsonResult JsonResultCustom(string data)
        {
            return new JsonResultCustom(data);
        }
        #endregion
    }
}