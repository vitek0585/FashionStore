using System;
using System.Linq.Expressions;
using System.Reflection;

namespace FashionStore.Infastructure.Data.Service.Store.Common
{
    public abstract class EntityServiceBase
    {
        #region Additional Methods

        protected PropertyInfo GetPropertyName<TItem>(string lang, Expression<Func<TItem, string>> expression)
            where TItem : class
        {
            var name = GetNameLamdaBody(lang, expression);
            var property = typeof(TItem).GetProperty(name,
                BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
            return property;
        }

        protected string GetNameByCurrentLangForDynamicType<TItem>(dynamic g, string lang,
            Expression<Func<TItem, string>> expression)
            where TItem : class
        {
            var name = GetNameLamdaBody(lang, expression);
            try
            {
                return g.GetType()
                    .GetProperty(name, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance)
                    .GetValue(g, null).ToString();
            }
            catch (Exception e)
            {
                return null;
            }
        }

        private static string GetNameLamdaBody<TItem>(string lang, Expression<Func<TItem, string>> expression)
            where TItem : class
        {
            var exp = (MemberExpression)expression.Body;
            var str = exp.Member.Name;
            return string.Concat(str.Substring(0, str.Length - 2), lang);
        }

        #endregion 
    }
}