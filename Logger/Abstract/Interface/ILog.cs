using System;

namespace WebLogger.Abstract.Interface
{
    public interface ILog
    {
        bool Remove(int id);
    }
    public interface ILog<T>:ILogWriter<T>,IDisposable
    {
       
    }
}