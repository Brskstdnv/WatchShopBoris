using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

using WatchShop.Infrastructure.Data.Domain;

namespace WatchShopApp.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {

        //protected override void OnModelCreating(ModelBuilder modelBuilder)
        //{
        //    base.OnModelCreating(modelBuilder);

        //    modelBuilder.Entity<ShoppingCartItem>()
        //        .HasOne(s => s.Order)
        //        .WithMany()
        //        .HasForeignKey(s => s.OrderId)
        //        .OnDelete(DeleteBehavior.Restrict);
        //}
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
            this.Database.EnsureCreated();  
        }
       


        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Manufacturer> Manufacturers { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<ShoppingCartItem> ShoppingCarts { get; set;}
    }
}
