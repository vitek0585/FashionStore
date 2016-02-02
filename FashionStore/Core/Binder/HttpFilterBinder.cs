using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web.Http.Controllers;
using System.Web.Http.ModelBinding;
using FashionStore.Domain.Core.Entities.Store;
using FilterWeb.FilterBinder;

namespace FashionStore.Core.Binder
{
    public class HttpFilterBinder : IModelBinder
    {
        private ConditionalGeneratorSimple<Good> _generator;

        public bool BindModel(HttpActionContext actionContext, ModelBindingContext bindingContext)
        {
            var values = actionContext.Request.RequestUri.ParseQueryString();
            try
            {
                _generator = new ConditionalGeneratorSimple<Good>(values);
                
                _generator.SetKeyValueExpression<IEnumerable<int>, IEnumerable<int>>
                    ((g, c, s) => g.ClassificationGoods.Any(cat => c.Contains(cat.ColorId) && s.Contains(cat.SizeId)), "colors", "sizes");
                _generator.SetKeyValueExpression<IEnumerable<int>>((g, c) => g.ClassificationGoods.Any(cat => c.Contains(cat.ColorId)), false, "colors");
                _generator.SetKeyValueExpression<IEnumerable<int>>((g, s) => g.ClassificationGoods.Any(cat => s.Contains(cat.SizeId)), false, "sizes");

                _generator.SetKeyValueExpression<decimal>((g, p) => Math.Ceiling(g.PriceUsd - (g.Discount ?? 0) / (decimal)100 * g.PriceUsd) >= p, "priceMin");
                _generator.SetKeyValueExpression<decimal>((g, p) => g.PriceUsd - (g.Discount ?? 0) / (decimal)100 * g.PriceUsd <= p, "priceMax");

                bindingContext.Model = _generator.GetConditional();

                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }
    }
}