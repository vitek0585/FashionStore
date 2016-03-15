using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Linq.Expressions;

namespace FilterWeb.FilterBinder
{
    public enum Order : byte
    {
        Asc = 1,
        Desc = 2
    }
    public class ConditionalOrderGenerator<TItem>
    {
        protected NameValueCollection _values;
        protected Lazy<List<ContainerExpression>> _expressions =
            new Lazy<List<ContainerExpression>>(() => new List<ContainerExpression>());

        protected string _key;
        public ConditionalOrderGenerator(NameValueCollection dic, string key)
        {
            _key = key;
            _values = dic;
        }
        public void SetKeyValueExpression(Expression<Func<TItem, object>> expr, string key, Order order = Order.Asc)
        {
            var orderStr = order == Order.Asc ? "ascending" : "descending";
            var query = ParseLamda(expr, orderStr);
            _expressions.Value.Add(new ContainerExpression(query, key));
        }
        public void SetKeyValueExpression(string expr, string key, Order order = Order.Asc)
        {
            var orderStr = order == Order.Asc ? "ascending" : "descending";
            var query = string.Join(" ", expr, orderStr);
            _expressions.Value.Add(new ContainerExpression(query, key));
        }
        private string ParseLamda(Expression<Func<TItem, object>> expr, string order)
        {
            var exp = (UnaryExpression)expr.Body;
            var str = ((MemberExpression)exp.Operand).Member.Name;
            return string.Join(" ", str, order);
        }
        public string GetConditional()
        {
            if (_values[_key] != null && _values[_key].Any() &&
                _expressions.Value.Any(i => i.Key.Equals(_values[_key],StringComparison.OrdinalIgnoreCase)))
            {
                return _expressions.Value.
                    FirstOrDefault(i => i.Key.Equals(_values[_key], StringComparison.OrdinalIgnoreCase)).Expression;
            }

            return _expressions.Value.FirstOrDefault().Expression;
        }

        #region Container for expression
        protected struct ContainerExpression
        {
            public string Expression { get; set; }
            public string Key { get; set; }

            public ContainerExpression(string expression, string key)
                : this()
            {
                Expression = expression;
                Key = key;

            }
        }
        #endregion
    }
}