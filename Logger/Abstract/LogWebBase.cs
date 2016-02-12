using System;
using System.Web;
using WebLogger.Abstract.Interface;
using WebLogger.Enum;

namespace WebLogger.Abstract
{
    public abstract class LogWebBase<T> : ILog, ILog<T>
    {
        protected string HttpMethod
        {
            get
            {
                return HttpContext.Current.Request.HttpMethod;
            }
        }
        protected string Path
        {
            get
            {
                return HttpContext.Current.Request.Path;
            }
        }
        protected string UrlReferrer
        {
            get
            {
                if (HttpContext.Current.Request.UrlReferrer != null)
                    return HttpContext.Current.Request.UrlReferrer.AbsolutePath;
                return null;
            }
        }
        protected string UserAgent
        {
            get
            {
                return HttpContext.Current.Request.UserAgent;
            }
        }
        protected bool IsAuthenticated
        {
            get
            {
                return HttpContext.Current.Request.IsAuthenticated;
            }
        }
        protected abstract void Execute(TypeLog typeLog, T message, Exception exception);
        public void LogWriteInfo(T message)
        {
            Execute(TypeLog.Info, message, null);
        }
        public void LogWriteWrong(T message)
        {
            Execute(TypeLog.Wrong, message, null);
        }

        public void LogWriteError(T message, Exception exception)
        {
            Execute(TypeLog.Error, message, exception);
        }

        public abstract bool Remove(int id);


        public abstract void Dispose();
    }
}