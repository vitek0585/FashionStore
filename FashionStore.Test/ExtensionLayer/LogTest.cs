using System.Linq;
using Moq;
using NUnit.Framework;
using WebLogger.Abstract.Interface;
using WebLogger.WebLog;


namespace FashionStore.Test.ExtensionLayer
{
    [TestFixture]
    public class LogTest
    {
       
        [Test]
        public void LogReadByPage()
        {
            Mock<IRequestContext> rc = new Mock<IRequestContext>();
           LogSql log = new LogSql(rc.Object,"ShopContext");
            log.ReadRange(0, 10);

        }
       
    }
}