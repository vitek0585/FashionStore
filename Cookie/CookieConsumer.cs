using System;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web;
using WebCookie.Interfaces;

namespace WebCookie
{

    public class CookieConsumer : ICookieConsumer
    {

        public void SetValueStorage(HttpContextBase context, string key, string value, string[] itemsContains)
        {
            if (itemsContains.Contains(value))
            {
                //var cookie = new CookieHeaderValue("session-id", "12345");
                //cookie.Expires = DateTimeOffset.Now.AddDays(1);
                //cookie.Domain = Request.RequestUri.Host;
                //cookie.Path = "/";
                HttpCookie cookie = new HttpCookie(key, value);
                cookie.HttpOnly = true;
                context.Response.Cookies.Add(cookie);
                //context.Response.Cookies[key].Value = value;
            }
        }

        public string GetValueStorage(HttpContextBase context, string key)
        {
            if (context.Request.Cookies.AllKeys.Contains(key))
                return context.Request.Cookies[key].Value;

            return null;
        }
        public string GetValueStorage(HttpRequestHeaders context, string key)
        {
            
            var cookie = context.GetCookies(key).FirstOrDefault();
            if (cookie != null)
                return cookie[key].Value;

            return null;
        }

        public string GetValueStorage(HttpCookieCollection cookies, string key)
        {
            var isComntain = cookies.AllKeys.Contains(key);
            if (isComntain)
            {
                return cookies[key].Value;
            }
            return null;
        }
    }
}