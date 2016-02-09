using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Web.Mvc;
using FashionStore.Infrastructure.Data.Context.Store.Context;
using Newtonsoft.Json;
using NUnit.Framework;

namespace FashionStore.Test.Data
{
    [TestFixture]
    public class DataBuilder
    {
        static public string PathToData
        {
            get
            {
                return Path.Combine(Environment.CurrentDirectory, "Data for test");
            }
        }

        [Test]
        public void Init()
        {
            BuildGoods();
            BuildClassificationGoods();
            BuildSizes();
            BuildColors();
            BuildCategories();
        }
        public void BuildGoods()
        {
            var c = new ShopContext();
            c.Configuration.ProxyCreationEnabled = false;
            var file = GetNameData(() => c.Goods);
            MakeJson(file, c.Goods.Take(100));
        }
        public void BuildSizes()
        {
            var c = new ShopContext();
            c.Configuration.ProxyCreationEnabled = false;
            var file = GetNameData(() => c.Sizes);
            MakeJson(file, c.Sizes.Take(100));
        }
        public void BuildColors()
        {
            var c = new ShopContext();
            c.Configuration.ProxyCreationEnabled = false;
            var file = GetNameData(() => c.Colors);
            MakeJson(file, c.Colors.Take(100));
        }
        public void BuildClassificationGoods()
        {
            var c = new ShopContext();
            c.Configuration.ProxyCreationEnabled = false;
            var file = GetNameData(() => c.ClassificationGoods);
            MakeJson(file, c.ClassificationGoods.Take(100));
        }
        public void BuildCategories()
        {
            var c = new ShopContext();
            c.Configuration.ProxyCreationEnabled = false;
            var file = GetNameData(() => c.Categories);
            MakeJson(file, c.Categories.Take(100));
        }
        private string GetNameData(Expression<Func<object>> exp)
        {
            return ExpressionHelper.GetExpressionText(exp).Split('.')[1];
        }

        private void MakeJson<T>(string fileName, IEnumerable<T> query)
        {
            fileName = Path.ChangeExtension(fileName, "json");
            var path = Path.Combine(PathToData, fileName);
            
            var jsonSer = new JsonSerializer
            {
                Formatting = Formatting.Indented,
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            };


            using (var st = new FileStream(path, FileMode.OpenOrCreate))
            {
                using (var sw = new StreamWriter(st))
                {
                    using (var jw = new JsonTextWriter(sw))
                    {
                        jsonSer.Serialize(jw, query);
                    }
                }
            }
        }
    }
}