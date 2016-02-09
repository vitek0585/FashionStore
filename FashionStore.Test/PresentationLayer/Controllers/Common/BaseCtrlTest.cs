using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq.Expressions;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.UI;
using AutoMapper;
using FashionStore.Configuration.Mapper;
using FashionStore.Test.Data;
using Moq;
using Newtonsoft.Json;
using NUnit.Framework;
using WebCookie.Interfaces;
using WebLogger.Abstract.Interface;


namespace FashionStore.Test.PresentationLayer.Controllers.Common
{
    public abstract class BaseCtrlTest
    {
        protected Mock<ICookieConsumer> _cookie = new Mock<ICookieConsumer>();
        protected Mock<ILogWriter<string>> _log = new Mock<ILogWriter<string>>();

        protected void InitMock()
        {
            _cookie.Setup(m => m.GetValueStorage(It.IsAny<HttpContextBase>(), It.IsAny<string>())).Returns(String.Empty);
            MapperConfig.SetupMap();

        }
       
        #region Additional methods

        #region Validate

        protected IList<ValidationResult> Validate(object model)
        {
            var results = new List<ValidationResult>();
            var validationContext = new ValidationContext(model, null, null);
            Validator.TryValidateObject(model, validationContext, results, true);
            if (model is IValidatableObject)
                (model as IValidatableObject).Validate(validationContext);
            return results;
        }

        #endregion


        #region Context fake

        protected ControllerContext CreateControllerContextFake(ControllerBase ctrl)
        {
            var request = new HttpRequest("", "http://example.com/", "");
            var response = new HttpResponse(TextWriter.Null);
            var httpContext = new HttpContextWrapper(new HttpContext(request, response));
            return new ControllerContext(httpContext, new RouteData(), ctrl);
        }

        #endregion


        #region Fake data
        
        protected IEnumerable<T> GetFakeData<T, TSource>(Expression<Func<TSource, object>> exp)
        {
            var name = GetNameData(exp);
            return TakeJson<T>(name);
        }

        protected string GetNameData<TSource>(Expression<Func<TSource, object>> exp)
        {
            return ExpressionHelper.GetExpressionText(exp);
        }

        protected string GetNameData<TSource>(Expression<Func<TSource, int>> exp)
        {
            var body = (MemberExpression) exp.Body;
            return body.Member.Name;
        }

        protected string GetNameData<TSource>(Expression<Func<TSource, bool>> exp)
        {
            var body = (UnaryExpression) exp.Body;
            return ((MemberExpression) body.Operand).Member.Name;
        }

        private IEnumerable<T> TakeJson<T>(string fileName)
        {
            fileName = Path.ChangeExtension(fileName, "json");
            var path = Path.Combine(DataBuilder.PathToData, fileName);
            var jsonSer = new JsonSerializer();


            using (var st = new FileStream(path, FileMode.Open))
            {
                using (var sw = new StreamReader(st))
                {
                    using (var jw = new JsonTextReader(sw))
                    {
                        return jsonSer.Deserialize<IEnumerable<T>>(jw);
                    }
                }
            }
        }

        #endregion

        #region Mapper
        protected TResult MapperType<TItem, TResult>(TItem data)
        {
            return Mapper.DynamicMap<TResult>(data);
        }
        protected IEnumerable<TResult> MapperCollection<TItem, TResult>(IEnumerable<TItem> data)
        {
            return Mapper.DynamicMap<IEnumerable<TResult>>(data);
        }
        #endregion

        #endregion 
    }
}