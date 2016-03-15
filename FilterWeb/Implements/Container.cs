using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using FilterWeb.Intarfaces;

namespace FilterWeb.Implements
{

    
    public struct ContainerExpression : IContainerFilterExpression
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
}