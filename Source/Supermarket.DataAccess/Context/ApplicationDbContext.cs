using Microsoft.EntityFrameworkCore;

using Supermarket.Domain.Entities;

namespace Supermarket.DataAccess.Context
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // One User has many Sales Information
            // One Sales Information has one User
            modelBuilder.Entity<SalesInformation>()
                        .HasOne(e => e.User)
                        .WithMany(e => e.SalesInformation)
                        .HasForeignKey(e => e.UserId);

            // One User has one Basket
            // One Basket has one User
            modelBuilder.Entity<User>()
                        .HasOne(x => x.Basket)
                        .WithOne(x => x.User)
                        .HasForeignKey<Basket>(x => x.UserId);

            // One Product has many Baskets
            // One Basket has many Products
            modelBuilder.Entity<ProductBasket>().HasKey(x => new { x.ProductId, x.BasketId });
        }

        public DbSet<User> Users { get; set; }
        public DbSet<SalesInformation> SalesInformation { get; set; }
        public DbSet<Basket> Baskets { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<ProductBasket> ProductBaskets { get; set; }
    }
}