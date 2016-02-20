using System.Web;
using WebLogger.Abstract.Interface;

namespace FashionStore.WorkFlow.Log
{
    public class RequestContextLog:IRequestContext
    {
        public string HttpMethod
        {
            get
            {
                return HttpContext.Current.Request.HttpMethod;
            }
        }

        public string Path
        {
            get
            {
                return HttpContext.Current.Request.Path;
            }
        }

        public string UrlReferrer
        {
            get
            {
                if (HttpContext.Current.Request.UrlReferrer != null)
                    return HttpContext.Current.Request.UrlReferrer.AbsolutePath;
                return null;
            }
        }

        public string UserAgent
        {
            get
            {
                return HttpContext.Current.Request.UserAgent;
            }
        }

        public bool IsAuthenticated
        {
            get
            {
                return HttpContext.Current.Request.IsAuthenticated;
            }
        }
    }
  
}