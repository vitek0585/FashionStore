using System;
using System.Web;
using System.Web.Mvc;
using FashionStore.Application.Bootstrapper.InversionOfControl;
using WebLogger.Abstract.Interface;
using WebLogger.Abstract.Interface.Sql;

namespace FashionStore.Core.HttpModule
{
    public class ErrorHandlerHttpModule : IHttpModule
    {
        Lazy<ILogWriterSql> _log =
            new Lazy<ILogWriterSql>(IoC.Resolve<ILogWriterSql>);
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