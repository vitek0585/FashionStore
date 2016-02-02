using System;

namespace WebLogger.Abstract.Interface
{
    public interface ILogWriter<in T>
    {
        void LogWriteInfo(T message);
        void LogWriteWrong(T message);
        void LogWriteError(T message, Exception exception);
    }
}