using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Linq.Expressions;

namespace FilterWeb.FilterBinder
{

    public class ConditionalGeneratorSimple<TItem>
    {

        protected NameValueCollection _values;
        protected Lazy<List<ContainerExpression>> _expressions =
            new Lazy<List<ContainerExpression>>(() => new List<ContainerExpression>());
        public ConditionalGeneratorSimple(NameValueCollection dic)
        {
            _values = dic;

        }

        #region Set up conditional
        /// <summary>
        /// Generate container and is adding to list that contains all the expressions
        /// </summary>
        /// <typeparam name="TConst">value filter that goes comparison</typeparam>
        /// <param name="expr">expression</param>
        /// <param name="require">if exists the expression with the same key (false - the expression will not be use)
        /// (true - the expression will be use). The order plays role</param>
        /// <param name="key">key from query string</param>
        public void SetKeyValueExpression<TConst>(Expression<Func<TItem, TConst, bool>> expr, bool require,
            params string[] key)
        {
            _expressions.Value.Add(new ContainerExpression(expr, expr.Parameters, key, require));
        }

        public void SetKeyValueExpression<TConst, TConst1>(Expression<Func<TItem, TConst, TConst1, bool>> expr,
            bool require, params string[] key)
        {
            _expressions.Value.Add(new ContainerExpression(expr, expr.Parameters, key, require));
        }

        public void SetKeyValueExpression<TConst>(Expression<Func<TItem, TConst, bool>> expr, params string[] key)
        {
            _expressions.Value.Add(new ContainerExpression(expr, expr.Parameters, key));
        }

        public void SetKeyValueExpression<TConst, TConst1>(Expression<Func<TItem, TConst, TConst1, bool>> expr,
            params string[] key)
        {
            _expressions.Value.Add(new ContainerExpression(expr, expr.Parameters, key));
        }

        #endregion

        public virtual Expression<Func<TItem, bool>> GetConditional()
        {
            try
            {
                
                var items = from item in _expressions.Value
                            where item.Keys.All(k => _values[k] != null && _values[k].Any())

                            let values = (from key in item.Keys
                                          select new
                                          {
                                              key,
                                              type = item.Zip[key],
                                              value = _values[key]
                                          })
                            select new
                            {
                                keys = values.Select(v => v.key),
                                expr = item.Expression,
                                values = values.ToList(),
                                require = item.IsRequire
                            };

                var keys = items.Where(i => i.require).SelectMany(i => i.keys);
                var query = items.Where(i => i.require || !i.keys.Any(k => keys.Contains(k)));

                Expression<Func<TItem, bool>> predicate = t => true;

                predicate = query.Aggregate(predicate, (pr, item) =>
                    And(pr, Expression.Invoke(item.expr,
                    Enumerable.Concat(new[] { predicate.Parameters[0] }, item.values.Select(v => SetupValue(v.value, v.type))))));

                return predicate;
            }
            catch (Exception e)
            {
                return t => false;
            }
        }
        protected Expression SetupValue(string val, Type type)
        {
            if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(IEnumerable<>))
            {
                var array = val.Split(',').Select(e => Expression.Constant(Convert.ChangeType(e, type.GenericTypeArguments[0])));
                return Expression.NewArrayInit(type.GenericTypeArguments[0], array);
            }
            var result = Convert.ChangeType(val, type);
            return Expression.Constant(result);
        }
        protected Expression<Func<T, bool>> And<T>(Expression<Func<T, bool>> expr1, InvocationExpression invokedExpr)
        {
            return Expression.Lambda<Func<T, bool>>
                  (Expression.AndAlso(expr1.Body, invokedExpr), expr1.Parameters);
        }
        #region Container for expression
        protected struct ContainerExpression
        {
            public Expression Expression { get; set; }
            public IEnumerable<string> Keys { get; set; }
            public Dictionary<string, Type> Zip { get; set; }
            public bool IsRequire { get; set; }
            public ContainerExpression(Expression expression, ICollection<ParameterExpression> type,
                IEnumerable<string> keys, bool require = true)
                : this()
            {
                IsRequire = require;
                Expression = expression;
                Keys = keys;
                var types = type.Select(t => t.Type).Skip(1);
                Zip = types.Zip(keys, (t, k) => new { t, k }).ToDictionary(a => a.k, a => a.t);
            }
        }
        #endregion

    }

}

