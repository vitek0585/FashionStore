using System;

namespace WebLogger.Abstract.Interface
{
    public interface ILog: IDisposable
    {
        bool Remove(int id);
    }
    
}