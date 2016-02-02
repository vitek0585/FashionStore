using System;

namespace WebLogger.Abstract.Interface
{
    public interface ILog<T>:ILogWriter<T>,IDisposable
    {
        
    }
}