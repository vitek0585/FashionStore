using System;
using System.Web;
using System.Web.Mvc;
using WebLogger.Abstract.Interface;

namespace FashionStore.Core.HttpModule
{
    public class ErrorHandlerHttpModule : IHttpModule
    {
        Lazy<ILogWriter<string>> _log =
            new Lazy<ILogWriter<string>>(() => DependencyResolver.Current.GetService<ILogWriter<string>>());
        public void Init(HttpApplication context)
        {
            context.Error += ErrorHandler;
        }

        private void ErrorHandler(object sender, EventArgs e)
        {
            
            var exc = HttpContext.Current.Server.GetLastError();
            if (exc is HttpException && ((HttpException)exc).GetHttpCode() == 404)
            {
                return;
            }

            _log.Value.LogWriteError("error handler", exc);
        }

        public void Dispose()
        {
            ((IDisposable)_log.Value).Dispose();
        }
    }
}