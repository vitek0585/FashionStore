using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Dynamic;
using System.Linq.Expressions;
using System.Web.Mvc;
using FashionStore.WorkFlow.BreadCrumbs;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace FashionStore.Utils.Helpers
{
    public static class HtmlExtensionsBreadCrumbs
    {
        public static MvcHtmlString GenerateBreadCrumbs(this HtmlHelper html, BreadCrumbsCompose compose)
        {
            return compose.Construct();
        }

        
    }

    public static class HtmlExtensionsMapToJson
    {
        public static string SerializeToJson<TItem>(this HtmlHelper html, TItem item)
        {
            var settings = new JsonSerializerSettings()
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            };
            return JsonConvert.SerializeObject(item, settings);
        }

    }
    public static class DynamicExtensions
    {
        public static dynamic ToDynamic(this object value)
        {
            IDictionary<string, object> expando = new ExpandoObject();

            foreach (PropertyDescriptor property in TypeDescriptor.GetProperties(value.GetType()))
                expando.Add(property.Name, property.GetValue(value));

            return expando as ExpandoObject;
        }
    }
    public static class ExtesionString
    {
        public static string ControllerAs(this string name,string ctrlAs)
        {
            if (string.IsNullOrEmpty(ctrlAs))
                return name;

            return string.Format("{0}.{1}", ctrlAs, name);
        }
        public static string GetName<T>(this HtmlHelper<T> html, Expression<Func<T, object>> exp)
        {
            return ExpressionHelper.GetExpressionText(exp);
        }
        public static string GetName<T>(this HtmlHelper html, Expression<Func<T, object>> exp, int? endRemove = null)
        {
            if (exp.Body.NodeType == ExpressionType.Convert || exp.Body.NodeType == ExpressionType.ConvertChecked)
            {
                var uexp = (UnaryExpression)exp.Body;
                return FirstCharacterToLow(((MemberExpression)uexp.Operand).Member.Name);
            }
            var str = FirstCharacterToLow(ExpressionHelper.GetExpressionText(exp));
            if (endRemove.HasValue)
                return str.Substring(0, str.Length - endRemove.Value);

            return str;
        }

        private static string FirstCharacterToLow(string name)
        {
            return Char.ToLowerInvariant(name[0]) + name.Substring(1);
        }

    }
}