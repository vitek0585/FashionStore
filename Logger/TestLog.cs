using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System.Web;
using NUnit.Framework;
using WebLogger.Concreate;

namespace WebLogger
{
    [TestFixture]
    public class TestLog
    {
        [Test]
        public async void WriteLog()
        {
            List<Task> list = new List<Task>();
            HttpContext.Current = new HttpContext(new HttpRequest("", "http://test.org", ""), 
                new HttpResponse(new StringWriter()));
            for (int i = 0; i < 1; i++)
            {
                var t = Task.Factory.StartNew(Execute);
                list.Add(t);

            }
            await Task.WhenAll(list);
        }

        private void Execute()
        {
            for (int i = 0; i < 100; i++)
            {
                Task.Factory.StartNew(() =>
                {
                    LogWebSql logWeb = new LogWebSql();
                    logWeb.LogWriteInfo("test message");
                    logWeb.Dispose();
                    
                }, TaskCreationOptions.AttachedToParent);

            }
        }

        [Test]
        public void TestLogSql()
        {
           
            HttpContext.Current = new HttpContext(new HttpRequest("", "http://test.org", ""),
                new HttpResponse(new StringWriter()));
            
            LogWebSql logWeb = new LogWebSql();
            logWeb.LogWriteInfo("TestLogSql message");
            logWeb.Dispose();
        }
        [Test]
        public void TestLogExplicitDispose()
        {
            HttpContext.Current = new HttpContext(new HttpRequest("", "http://test.org", ""),
                new HttpResponse(new StringWriter()));

            LogWebSql logWeb = new LogWebSql();
            logWeb.LogWriteInfo("TestLogSql message");
            GC.Collect();
            GC.WaitForPendingFinalizers();

        }
        [Test]
        public void TestLogImplicitDispose()
        {
            HttpContext.Current = new HttpContext(new HttpRequest("", "http://test.org", ""),
                new HttpResponse(new StringWriter()));

            LogWebSql logWeb = new LogWebSql();
            logWeb.LogWriteInfo("TestLogSql message");
            logWeb.Dispose();
            GC.Collect();
            GC.WaitForPendingFinalizers();

        }
        [Test]
        public void TestLogWrite()
        {
            HttpContext.Current = new HttpContext(new HttpRequest("", "http://test.org", ""),
                new HttpResponse(new StringWriter()));

            LogWebSql logWeb = new LogWebSql();
            logWeb.LogWriteError("TestLogSql message", new AggregateException(new Exception("Add Test exception")));
           

        }
    }
}