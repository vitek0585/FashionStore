﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using FashionStore.Domain.Core.Entities.Store;
using FashionStore.Domain.Interfaces.Repository;
using FashionStore.Infrastructure.Data.Context.Store.Context;
using FashionStore.Infrastructure.Data.Repository.Common;

namespace FashionStore.Infrastructure.Data.Repository.Store
{
    public class SalePosRepository : GlobalRepository<SalePos>, ISalePosRepository
    {
        public SalePosRepository(ShopContext context)
            : base(context)
        {
        }


        public void Add(IEnumerable<SalePos> sales, IEnumerable<int> ids, Sale sale, string error)
        {
            _context.Configuration.AutoDetectChangesEnabled = true;
            _context.Configuration.ProxyCreationEnabled = true;

            var infoGoods = _context.Set<ClassificationGood>().Where(c => ids.Contains(c.ClassificationId)).AsEnumerable();

            foreach (var spos in sales)
            {
                var typeGood = infoGoods.First(i => i.ColorId == spos.ColorId && i.SizeId == spos.SizeId);
                if (typeGood.CountGood - spos.CountGood < 0)
                {
                    throw new ArgumentException(
                         String.Format(error, GetLanguage() == "ru" ?
                         typeGood.Good.GoodNameRu : typeGood.Good.GoodNameEn, typeGood.CountGood.ToString()));
                }
                typeGood.CountGood -= spos.CountGood;
                sale.SalePos.Add(spos);
            }
        }

        private string GetLanguage()
        {
            return Thread.CurrentThread.CurrentCulture.TwoLetterISOLanguageName;
        }
    }
}