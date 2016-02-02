using System.Net.Http.Headers;
using System.Web;

namespace WebCookie.Interfaces
{
    public interface ICookieConsumer
    {
        void SetValueStorage(HttpContextBase context, string key, string value, string[] itemsContains);
        string GetValueStorage(HttpContextBase context, string key);
        string GetValueStorage(HttpRequestHeaders context, string key);
        string GetValueStorage(HttpCookieCollection cookies, string key);
    }
}