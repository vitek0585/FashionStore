using System.Data.Entity.Migrations;
using FashionStore.Infastructure.Data.Identity.Context;

namespace FashionStore.Infastructure.Data.Identity.Migrations
{
    internal sealed class Configuration : DbMigrationsConfiguration<DbContextIdentity>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
        }

        protected override void Seed(DbContextIdentity context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data. E.g.
            //
            //    context.People.AddOrUpdate(
            //      p => p.FullName,
            //      new Person { FullName = "Andrew Peters" },
            //      new Person { FullName = "Brice Lambson" },
            //      new Person { FullName = "Rowan Miller" }
            //    );
            //
        }
    }
}
