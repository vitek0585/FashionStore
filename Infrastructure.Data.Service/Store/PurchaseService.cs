using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using FashionStore.Domain.Core.Entities.Store;
using FashionStore.Domain.Interfaces.Repository;
using FashionStore.Infrastructure.Data.Service.Resource;
using FashionStore.Infrastructure.Data.Service.Store.Common;
using FashionStore.Service.Interfaces.Results;
using FashionStore.Service.Interfaces.Services;
using FashionStore.Service.Interfaces.UoW;

namespace FashionStore.Infrastructure.Data.Service.Store
{
    public class PurchaseService : EntityServiceBase, IPurchaseService
    {
        private IUnitOfWork _unit;
        private IGoodsRepository _goods;
        private IClassificationGoodRepository _classificationGood;
        private ISalePosRepository _salePos;
        private ISaleRepository _sale;
        private IExchangeRatesService _exchangeRates;
        public PurchaseService(IGoodsRepository good, IClassificationGoodRepository classificationGood, IUnitOfWorkStore unit,
            ISaleRepository sale, ISalePosRepository salePos, IExchangeRatesService exchangeRates)
        {
            _exchangeRates = exchangeRates;
            _sale = sale;
            _salePos = salePos;
            _classificationGood = classificationGood;
            _goods = good;
            _unit = unit;
        }
        public ClassificationGood GetClassification(ClassificationGood good)
        {
            return _classificationGood.DisabledProxy<IClassificationGoodRepository>().GetAll()
                .FirstOrDefault(
                    g =>
                        g.GoodId == good.GoodId && g.SizeId == good.SizeId && g.ColorId == good.ColorId &&
                        g.CountGood > 0);

        }

        public PurchaseResult MakeAnOrder(IEnumerable<ClassificationGood> list, int? userId, string userName, string phone, string email)
        {
            try
            {
                _unit.StartTransaction();

                var sale = GetSale(userId, userName, phone, email);
                _sale.EnableProxy<ISaleRepository>().Add(sale);
                var poses = GetSalePoses(list);
                var idsCls = list.Select(i => i.ClassificationId);
         
                _salePos.Add(poses, idsCls, sale, Messages.NoEnoughtGoods);

                _unit.Save();
                _unit.Commit();
            }
            catch (ArgumentException e)
            {
                _unit.Rollback();
                return new PurchaseResult(e.Message);
            }
            catch (Exception e)
            {
                _unit.Rollback();
                return new PurchaseResult(Messages.OrderError, e);
            }
            return new PurchaseResult();
        }

        public IEnumerable<TResult> GetGoodsByCart<TResult>(IEnumerable<ClassificationGood> list, string currentCurrency, string lang)
        {
            var rate = _exchangeRates.GetActualRateUsdWithUpdateIfNotExist();
            var idsCls = list.Select(g => g.ClassificationId);
            var data = _classificationGood.DisabledProxy<IClassificationGoodRepository>().GetAll()
                .Where(c => idsCls.Contains(c.ClassificationId))
                .Select(c => new
                {
                    c.ClassificationId,
                    c.ColorId,
                    c.SizeId,
                    c.Color.ColorNameEn,
                    c.Size.SizeName,

                    c.Good.GoodNameEn,
                    c.Good.GoodNameRu,
                    c.Good.PriceUsd,
                    c.Good.GoodId,
                    photos = c.Good.Image.Select(i => i.ImagePath),

                    Discount = c.Good.Discount ?? 0,

                }).AsEnumerable()
                .Select(c => new
                {
                    c.GoodId,
                    c.ClassificationId,
                    c.SizeId,
                    c.ColorId,
                    c.photos,
                    colorName = c.ColorNameEn,
                    c.SizeName,
                    GoodName = GetNameByCurrentLangForDynamicType<Good>(c, lang, g => g.GoodNameEn),
                    priceUsd =
                    _exchangeRates.ConvertWithCeilingUsdToWithDiscount(c.PriceUsd, c.Discount, rate, currentCurrency),
                    priceWithDiscount = _exchangeRates.ConvertWithCeilingUsdToWithDiscount(c.PriceUsd, c.Discount, rate, currentCurrency),
                    c.Discount,
                    countGood = list.First(i => i.ClassificationId == c.ClassificationId).CountGood
                });

            return Mapper.DynamicMap<IEnumerable<TResult>>(data);
        }

        public IEnumerable<TResult> GetGoodsDetails<TResult>(int id)
        {
            var data = _classificationGood.DisabledProxy<IClassificationGoodRepository>()
                .GetByExpressionSelectList((g) => g.GoodId == id,
                   g => new
                   {
                       g.ClassificationId,

                       g.GoodId,
                       g.Color.ColorId,
                       ColorName = g.Color.ColorNameEn,
                       g.Size.SizeId,
                       g.Size.SizeName,
                       g.CountGood
                   },
                   g => g.Size,
                   g => g.Color);

            return Mapper.DynamicMap<IEnumerable<TResult>>(data);


        }
        #region Additional Method

        private IEnumerable<SalePos> GetSalePoses(IEnumerable<ClassificationGood> list)
        {
            var idsGoods = list.Select(g => g.GoodId);

            var goods = _goods.DisabledProxy<IGoodsRepository>().GetAll()
                .Where(g => idsGoods.Contains(g.GoodId)).AsEnumerable();

            var poses = list.Select(p => new SalePos()
            {
                GoodId = p.GoodId,
                SizeId = p.SizeId,
                ColorId = p.ColorId,
                CountGood = p.CountGood,
                Price = goods.First(g => g.GoodId == p.GoodId).PriceUsd,
                Discount = goods.First(g => g.GoodId == p.GoodId).Discount
            });
            return poses;
        }

        private Sale GetSale(int? userId, string userName, string phone, string email)
        {
            var sale = new Sale()
            {
                DateSale = DateTime.Now,
                UserName = userName,
                Email = email,
                Phone = phone,
                UserId = userId
            };
            return sale;
        }

        #endregion



    }
}