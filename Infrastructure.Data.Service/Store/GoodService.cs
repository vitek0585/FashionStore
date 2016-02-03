using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using AutoMapper;
using System.Linq.Dynamic;
using FashionStore.Domain.Core.Entities.Store;
using FashionStore.Domain.Interfaces.Repository;
using FashionStore.Infastructure.Data.Service.Store.Common;
using FashionStore.Service.Interfaces.Services;
using FashionStore.Service.Interfaces.UoW;
using LinqKit;

namespace FashionStore.Infastructure.Data.Service.Store
{
    public class GoodService : EntityService<Good>, IGoodService
    {
        private IGoodsRepository _goods;
        private ICategoryRepository _category;
        private IExchangeRatesService _exchangeRates;

        public GoodService(IUnitOfWorkStore unitOfWork, IGoodsRepository repository, ICategoryRepository category,
            IExchangeRatesService exchangeRates)
            : base(unitOfWork, repository)
        {
            _category = category;
            _goods = repository;
            _exchangeRates = exchangeRates;
        }

        public TResult GetGood<TResult>(int id, string currentCurrency, string lang)
        {
            var rate = _exchangeRates.GetActualRateUsdWithUpdateIfNotExist();
            var good = _goods.DisabledProxy<IGoodsRepository>().GetByExpressionSelect((g) => g.GoodId == id,
                   g => new
                   {
                       g.GoodId,
                       g.CategoryId,
                       Discount = g.Discount ?? 0,
                       g.GoodNameRu,
                       g.GoodNameEn,
                       // GoodName = propName.GetValue(g),
                       goodCount = g.ClassificationGoods.Sum(c => c.CountGood),
                       g.PriceUsd,
                       //priceUsd = _exchangeRates.ConvertWithCeilingUsdTo(g.PriceUsd, currentCurrency),
                       photos = g.Image.Select(p => p.ImagePath),
                       types = g.ClassificationGoods.Where(c => c.CountGood > 0).Select(c => new
                       {
                           c.Color.ColorId,
                           ColorName = c.Color.ColorNameEn,
                           c.Size.SizeId,
                           c.Size.SizeName,
                           c.CountGood
                       }),
                       g.DescriptionRu,
                       g.DescriptionEn,
                       //description = propDesc.GetValue(g)
                   },
                   g => g.Image, g => g.ClassificationGoods,
                   g => g.ClassificationGoods.Select(c => c.Size),
                   g => g.ClassificationGoods.Select(c => c.Color));

            var data = new
            {
                good.GoodId,
                good.Discount,
                good.CategoryId,
                priceUsd =
                    _exchangeRates.ConvertWithCeilingUsdTo(good.PriceUsd, rate,currentCurrency),
                PriceWithDiscount = _exchangeRates.ConvertWithCeilingUsdToWithDiscount(good.PriceUsd, good.Discount, rate, currentCurrency),
                good.goodCount,
                good.photos,
                good.types,

                description = GetNameByCurrentLangForDynamicType<Good>(good, lang, g => g.DescriptionEn),
                goodName = GetNameByCurrentLangForDynamicType<Good>(good, lang, g => g.GoodNameRu)

            };

            return Mapper.DynamicMap<TResult>(data);


        }
        public IEnumerable<TResult> GetRandomGoods<TResult>(int count, string currentCurrency, string lang)
        {

            var rand = new Random().Next(0, _goods.GetCount() - count + 1);
            var actualRate = _exchangeRates.GetActualRateUsdWithUpdateIfNotExist();

            var data = _goods.DisabledProxy<IGoodsRepository>().GetAll()
                .Select(g => new
                {
                    g.GoodId,
                    g.CategoryId,
                    g.GoodNameRu,
                    g.GoodNameEn,
                    g.PriceUsd,
                    g.Discount,
                    goodCount = g.ClassificationGoods.Select(c => c.CountGood),
                    photos = g.Image.Select(i => i.ImagePath),


                }).OrderBy(g => g.GoodId)
                .Skip(() => rand).Take(() => count).AsEnumerable()
                .Select(g => ToProjection(g, actualRate, currentCurrency, lang));


            return DynamicMap<TResult>(data);

        }
        public IEnumerable<TResult> GetNewGoods<TResult>(int count, string currentCurrency, string lang)
        {
            var actualRate = _exchangeRates.GetActualRateUsdWithUpdateIfNotExist();
            var tmp = (from row in _goods.GetAll()
                orderby row.DateCreate descending 
                select row.GoodId).Take(() => 30).OrderBy(c => Guid.NewGuid()).Take(() => count).ToList();
            var data = (from g in _goods.GetAll()
                        select
                        new
                        {
                            g.GoodId,

                            g.Category.CategoryId,
                            g.Category.Type.TypeNameEn,
                            g.Category.Type.TypeNameRu,
                            g.Category.Name.CategoryNameEn,
                            g.Category.Name.CategoryNameRu,

                            g.GoodNameRu,
                            g.GoodNameEn,
                            g.PriceUsd,
                            g.Discount,
                            goodCount = g.ClassificationGoods.Select(c => c.CountGood),
                            photos = g.Image.Select(i => i.ImagePath),
                            PriceWithDiscount = g.PriceUsd - (g.Discount ?? 0) / (decimal)100 * g.PriceUsd

                        }).Where(g => tmp.Contains(g.GoodId)).AsEnumerable()
                        .Select(g => ToProjection(g, actualRate, currentCurrency, lang));

            return DynamicMap<TResult>(data);

        }
        public TResult GetByPage<TResult>(int page, int totalPerPage, int category,
            string currentCurrency, string lang, Expression<Func<Good, bool>> predicat, string ordering)
        {
            var ids = _category.GetCategoryAndChildId(category);
            var actualRate = _exchangeRates.GetActualRateUsdWithUpdateIfNotExist();

            Expression<Func<Good, bool>> f = g => ids.Contains(g.CategoryId.Value);
            var invokedExpr = Expression.Invoke(f, predicat.Parameters);
            var pr = Expression.Lambda<Func<Good, bool>>(Expression.AndAlso(predicat.Body, invokedExpr), predicat.Parameters);

            var totalCount = _goods.DisabledProxy<IGoodsRepository>()
                .GetAll().AsExpandable().Count(pr);

            decimal onehundred = 100;
            var data = _goods.DisabledProxy<IGoodsRepository>().GetAll().AsExpandable()
                .Where(pr)
                .Select(g => new
                {
                    g.GoodId,
                    g.GoodNameEn,
                    g.GoodNameRu,
                    Discount = g.Discount ?? 0,

                    g.Category.CategoryId,
                    g.Category.Type.TypeNameEn,
                    g.Category.Type.TypeNameRu,
                    g.Category.Name.CategoryNameEn,
                    g.Category.Name.CategoryNameRu,
                    g.DateCreate,
                    goodCount = g.ClassificationGoods.FirstOrDefault().CountGood,
                    g.PriceUsd,
                    photos = g.Image.Select(p => p.ImagePath),
                    //for sort
                    PriceWithDiscount = g.PriceUsd - (g.Discount ?? 0) / onehundred * g.PriceUsd
                })
                .OrderBy(ordering).Skip(totalPerPage * (page - 1)).Take(() => totalPerPage)
                .AsEnumerable()
                .Select(g => ToProjectionByPage(g, actualRate, currentCurrency, lang))
                .GroupBy(g => new { group = 0 })
                .Select(gr => new
                {
                    totalCount = totalCount,
                    totalPagesCount = Math.Ceiling((double)totalCount / totalPerPage),
                    items = gr.Select(g => ToProjectionGroupBy(g))

                }).FirstOrDefault();

            if (data == null)
            {
                return default(TResult);
            }

            return Mapper.DynamicMap<TResult>(data);
        }
        public IEnumerable<TResult> GetOrdersById<TResult>(IEnumerable<int> id, string currentCurrency, string lang)
        {
            var propName = GetPropertyName<Good>(lang, (g) => g.GoodNameRu);
            var actualRate = _exchangeRates.GetActualRateUsdWithUpdateIfNotExist();
            var data = _goods.DisabledProxy<IGoodsRepository>().GetAll()
                .Include(g => g.Image)
                .AsExpandable()
                .Where(g => id.Contains(g.GoodId)).Select(g => new
                   {
                       g.GoodId,
                       GoodName = propName.GetValue(g),
                       price = _exchangeRates.ConvertWithCeilingUsdTo(g.PriceUsd, actualRate, currentCurrency),
                       photo = g.Image.Select(p => p.ImagePath).FirstOrDefault()
                   });

            return DynamicMap<TResult>(data);
        }

        public IEnumerable<TResult> GetGoods<TResult>(IEnumerable<int> ids, string lang)
        {
            var data = _goods.DisabledProxy<IGoodsRepository>().GetAll()
                .Where(g => ids.Contains(g.GoodId)).Select(g => new
                {
                    g.GoodId,
                    g.GoodNameEn,
                    g.GoodNameRu,
                    photos = g.Image.FirstOrDefault().ImagePath

                }).AsEnumerable().Select(g => new
                {
                    g.GoodId,
                    GoodName = GetNameByCurrentLangForDynamicType<Good>(g, lang, good => good.GoodNameEn),
                    photos = g.photos != null ? Enumerable.Repeat(g.photos, 1) : null
                });

            return DynamicMap<TResult>(data);
        }

        #region Additional methods


        private dynamic ToProjection(dynamic g, decimal rate, string currentCurrency, string lang)
        {
            return new
            {
                g.GoodId,

                g.CategoryId,

                TypeName = GetNameByCurrentLangForDynamicType<CategoryType>((object)g, lang, cat => cat.TypeNameRu),
                CategoryName = GetNameByCurrentLangForDynamicType<CategoryName>((object)g, lang, cat => cat.CategoryNameRu),

                GoodName = GetNameByCurrentLangForDynamicType<Good>((object)g, lang, good => good.GoodNameEn),
                goodCount = Enumerable.Sum(g.goodCount),
                Discount = g.Discount ?? 0,
                g.photos,
                priceUsd = _exchangeRates.ConvertWithCeilingUsdTo(g.PriceUsd, rate, currentCurrency),
                PriceWithDiscount = _exchangeRates.ConvertWithCeilingUsdToWithDiscount(g.PriceUsd, g.Discount ?? 0, rate, currentCurrency),
            };
        }
        private dynamic ToProjectionGroupBy(dynamic g)
        {
            return new
            {
                g.GoodId,

                g.CategoryId,
                g.TypeName,
                g.CategoryName,

                g.GoodName,
                g.GoodCount,
                g.Discount,
                g.photos,
                g.priceUsd,
                g.PriceWithDiscount
            };
        }
        private dynamic ToProjectionByPage(dynamic g, decimal rate, string currentCurrency, string lang)
        {
            return new
            {
                g.GoodId,

                g.CategoryId,
                TypeName = GetNameByCurrentLangForDynamicType<CategoryType>((object)g, lang, cat => cat.TypeNameRu),
                CategoryName = GetNameByCurrentLangForDynamicType<CategoryName>((object)g, lang, cat => cat.CategoryNameRu),

                GoodName = GetNameByCurrentLangForDynamicType<Good>((object)g, lang, good => good.GoodNameEn),
                GoodCount = g.goodCount,
                Discount = g.Discount ?? 0,
                g.photos,
                priceUsd = _exchangeRates.ConvertWithCeilingUsdTo(g.PriceUsd, rate, currentCurrency),
                PriceWithDiscount = _exchangeRates.ConvertWithCeilingUsdToWithDiscount(g.PriceUsd, g.Discount, rate, currentCurrency),
            };
        }

        private static IEnumerable<TResult> DynamicMap<TResult>(IEnumerable<dynamic> data)
        {
            return Mapper.DynamicMap<IEnumerable<TResult>>(data);
        }

        #endregion


    }
}