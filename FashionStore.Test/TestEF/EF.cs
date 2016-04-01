using System.Data.Entity;
using FashionStore.Domain.Core.Entities.Store;
using FashionStore.Infrastructure.Data.Context.Store.Context;
using NUnit.Framework;

namespace FashionStore.Test.TestEF
{
    [TestFixture]
    public class Ef
    {
        [Test]
        public void EfAttach()
        {
            var context = new ShopContext();

            var color = new Color();
            color.ColorNameEn = "sky blue";
            color.ColorNameRu = "Светло синий";

            var colorAdded = context.Entry(color);
            colorAdded.State = EntityState.Added;
            context.SaveChanges();


        }
        [Test]
        public void EfEdit()
        {
            var context = new ShopContext();
           // context.Configuration.AutoDetectChangesEnabled = false;
            var colorEdit = context.Colors.Find(8);

            colorEdit.ColorNameEn = "sky blue 1000";
            colorEdit.ColorNameRu = "Светло синий - 10";
            context.Entry(colorEdit).Property(c=>c.ColorNameRu).IsModified = true;
            context.SaveChanges();


        }
       
    }
}