using System.Collections.Generic;

namespace WebLogger.Abstract.Interface
{
    public interface ILogReader<out T>
    {
        T AllLogs();
        T ReadRange(int startPos, int range);
    }
}