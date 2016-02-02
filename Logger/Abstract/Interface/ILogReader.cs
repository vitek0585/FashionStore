using System.Collections.Generic;

namespace WebLogger.Abstract.Interface
{
    public interface ILogReader<out TResult>
    {

        IEnumerable<TResult> LogRead();
    }
}