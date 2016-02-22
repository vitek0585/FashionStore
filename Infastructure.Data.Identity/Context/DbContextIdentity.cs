using System.Data.Entity;
using FashionStore.Infrastructure.Data.Identity.Entities;
using Microsoft.AspNet.Identity.EntityFramework;

namespace FashionStore.Infrastructure.Data.Identity.Context
{
    public class DbContextIdentity : IdentityDbContext<User, Role, int, UserExternLogin, UserRole, Claim>
    {
        public DbContextIdentity(string connection)
            : base(connection)
        {

        }
        public DbContextIdentity()
            : base("shopIdentity")
        {

        }
        public static IdentityDbContext Create(string connection)
        {
            return new IdentityDbContext(connection);
        }


        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>().ToTable("User");
            modelBuilder.Entity<User>().Property(u => u.UserName).HasColumnName("Login");
            modelBuilder.Entity<User>().Property(u => u.PasswordHash).HasColumnName("Password");

            modelBuilder.Entity<Role>().ToTable("Role");
            modelBuilder.Entity<Claim>().ToTable("Claim");
            modelBuilder.Entity<UserExternLogin>().ToTable("ExternLogin");
            modelBuilder.Entity<UserRole>().ToTable("UserRole");


        }
    }
}