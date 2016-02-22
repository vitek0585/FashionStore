using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace FilterWeb.Intarfaces
{
    public interface IContainerExpression<TExpression, TKeys>
    {
        TExpression Expression { get; set; }
        TKeys Keys { get; set; }

    }
    public interface IContainerFilterExpression : IContainerExpression<Expression, IEnumerable<string>>
    {
        Dictionary<string, Type> Zip { get; set; }
        bool IsRequire { get; set; }
    }
}