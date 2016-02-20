using System;
using System.Collections.Generic;
using WebLogger.Abstract.Interface;
using WebLogger.Type;

namespace WebLogger.Abstract
{
    

    public abstract class LogBase<TWrite, TRead> : ILogWriter<TWrite>, ILogReader<TRead>,ILog
    {
       protected abstract void Execute(TypeLog typeLog, TWrite message, Exception exception);
        public void LogWriteInfo(TWrite message)
        {
            Execute(TypeLog.Info, message, null);
        }
        public void LogWriteWrong(TWrite message)
        {
            Execute(TypeLog.Wrong, message, null);
        }

        public void LogWriteError(TWrite message, Exception exception)
        {
            Execute(TypeLog.Error, message, exception);
        }

        public abstract TRead AllLogs();
        public abstract TRead ReadRange(int startPos, int range);

        public abstract void Dispose();
        public abstract bool Remove(int id);
        
    }
}