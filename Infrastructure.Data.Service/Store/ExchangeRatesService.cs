using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.SqlServer;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Threading;
using FashionStore.Domain.Core.Entities.Store;
using FashionStore.Domain.Interfaces.Repository;
using FashionStore.Infrastructure.Data.Service.Store.Common;
using FashionStore.Service.Interfaces.Services;
using FashionStore.Service.Interfaces.UoW;
using LinqKit;
using Newtonsoft.Json;

namespace FashionStore.Infrastructure.Data.Service.Store
{
    public class ExchangeRatesService : EntityService<ExchangeRates>, IExchangeRatesService
    {
        private string _url;
        private const string _usd = "usd";
        private static object _sync = new object();
        private IExchangeRatesRepository _exchangeRates;
        public ExchangeRatesService(IUnitOfWorkStore unitOfWork, IExchangeRatesRepository repository, string url)
            : base(unitOfWork, repository)
        {
            _exchangeRates = repository;
            _url = url;
        }
        public decimal ConvertUsdTo(decimal value, decimal rate, string convertTo)
        {
            if (string.Equals(convertTo, "usd", StringComparison.OrdinalIgnoreCase))
            {
                return value;
            }

            if (string.Equals(convertTo, "uah", StringComparison.OrdinalIgnoreCase))
            {
                return rate * value;
            }

            throw new FormatException("Converter format was mistakes");
        }
        public decimal ConvertWithCeilingUsdTo(decimal value, decimal rate, string convertTo)
        {
            return Math.Ceiling(ConvertUsdTo(value, rate, convertTo));
        }
        public decimal ConvertWithCeilingUsdToWithDiscount(decimal value, decimal discount, decimal rate, string convertTo)
        {
            var result = Math.Ceiling(value - (discount/100)*value);
            return Math.Ceiling(ConvertUsdTo(result, rate, convertTo));
        }
        public decimal GetActualRateUsdWithUpdateIfNotExist(bool isUpdate = true, bool getPrev = false)
        {
            if (getPrev)
            {
                return GetAnyRate(isUpdate);
            }
            var today = GetRateFromDb(e => e.DateRate == DbFunctions.TruncateTime(SqlFunctions.GetDate()));

            if (isUpdate && today == null)
            {
                return GetRemoteActualRate();
            }
            try
            {
                return today.UsdRate;
            }
            catch (Exception e)
            {
                throw new NullReferenceException("Not found currency for today in the db");
            }

        }

        private decimal GetAnyRate(bool isUpdate)
        {
            var anyRate = GetRateFromDb(e => true);
            if (isUpdate && anyRate == null)
            {
                return GetRemoteActualRate();
            }
            try
            {
                return anyRate.UsdRate;
            }
            catch (Exception e)
            {
                throw new NullReferenceException("Not found any currency in the db");
            }
        }

        private decimal GetRemoteActualRate()
        {
            try
            {
                var isInit = false;
                Currency currency = null;
                LazyInitializer.EnsureInitialized(ref currency, ref isInit, ref _sync, GetActualExchangeRates);
                var newItem = Add(new ExchangeRates() { DateRate = DateTime.Now, UsdRate = currency.Sale });
                Save();
                return newItem.UsdRate;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        private ExchangeRates GetRateFromDb(Expression<Func<ExchangeRates, bool>> predicat)
        {
            var rate =
                _exchangeRates.DisabledProxy<IExchangeRatesRepository>().GetAll().AsExpandable()
                    .Where(predicat).OrderByDescending(e => e.DateRate).FirstOrDefault();

            return rate;
        }

        private Currency GetActualExchangeRates()
        {
            HttpWebRequest request = WebRequest.CreateHttp(_url);
            var result = request.GetResponse();
            IEnumerable<Currency> array;
            using (var stream = new StreamReader(result.GetResponseStream()))
            {
                array = JsonConvert.DeserializeObject<IEnumerable<Currency>>(stream.ReadToEnd());
            }
            return array.FirstOrDefault(c => c.Ccy.Equals(_usd, StringComparison.OrdinalIgnoreCase));
        }

        #region helper class for web request api

        private class Currency
        {
            public string Ccy { get; set; }
            public decimal Buy { get; set; }
            public decimal Sale { get; set; }

        }

        #endregion


    }
}