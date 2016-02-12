using System.Linq;
using NUnit.Framework;
using WebLogger.Abstract.Interface;
using WebLogger.Concreate;

namespace FashionStore.Test.ExtensionLayer
{
    [TestFixture]
    public class LogTest
    {
        private ILogReader _log = new LogWebSql();
        [Test]
        public void LogReadByPage()
        {
            var data = _log.LogReadPage(1, 10);
            var count = data.items.Count;
            Assert.That(count, Is.EqualTo(10));
            data = _log.LogReadPage(1, 10);
            count = data.items.Count;
            Assert.That(count, Is.EqualTo(10));
        }
       
    }
}