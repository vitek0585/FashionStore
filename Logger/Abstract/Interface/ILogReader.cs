using System.Collections.Generic;

namespace WebLogger.Abstract.Interface
{
    public interface ILogReader
    {
        IEnumerable<dynamic> LogRead();
        dynamic LogReadPage(int page, int perPage);
    }
}