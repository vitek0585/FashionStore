using System.Collections.Generic;
using System.Linq;
using FashionStore.Domain.Core.Entities.Store;
using FashionStore.Domain.Interfaces.Repository;
using FashionStore.Infastructure.Data.Service.Store.Common;
using FashionStore.Service.Interfaces.Services;
using FashionStore.Service.Interfaces.UoW;

namespace FashionStore.Infastructure.Data.Service.Store
{
    public class SaleService : EntityService<Sale>, ISaleService
    {
        private IExchangeRatesService _ratesService;
        public SaleService(IUnitOfWork unitOfWork, ISaleRepository repository, IExchangeRatesService ratesService)
            : base(unitOfWork, repository)
        {
            _ratesService = ratesService;
        }

        public IEnumerable<TResult> SaleByPage<TResult>(int id, int page, int totalPerPage, string currentCurrency, string lang)
        {
            var actualRate = _ratesService.GetActualRateUsdWithUpdateIfNotExist();

            var data = _repository.DisabledProxy<ISaleRepository>()
                .GetAll().Where(s => s.UserId == id)
                .Select(s => new
                {
                    s.SaleId,
                    s.DateSale,
                    goods = s.SalePos.Select(p => new
                    {
                        p.CountGood,
                        p.Price,
                        Discount = p.Discount ?? 0,
                        PriceWithDiscount = p.Price - (p.Discount ?? 0)/100*p.Price,
                        ClassificationId = p.ClassificationGood.ClassificationId,
                        GoodId = p.ClassificationGood.Good.GoodId,
                        GoodNameEn = p.ClassificationGood.Good.GoodNameEn,
                        GoodNameRu = p.ClassificationGood.Good.GoodNameRu,
                        photo = p.ClassificationGood.Good.Image.FirstOrDefault().ImagePath,

                    })
                }).OrderByDescending(s => s.DateSale).Skip(totalPerPage*(page - 1)).Take(totalPerPage)
                .AsEnumerable()
                .Select(s => new
                {
                    
                    s.SaleId,
                    s.DateSale,
                    goods = s.goods.Select(p => new
                    {
                        p.CountGood,
                        p.Price,
                        p.Discount,
                        p.ClassificationId,
                        p.GoodId,
                        GoodName = GetNameByCurrentLangForDynamicType<Good>(p, lang, g => g.GoodNameEn),
                        p.photo,
                        PriceWithDiscount =
                            _ratesService.ConvertWithCeilingUsdToWithDiscount(p.Price, p.Discount, actualRate,
                                currentCurrency)
                    })
                });

            return MapCollection<TResult>(data);
        }
    }
}