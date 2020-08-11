using System;
using Microsoft.EntityFrameworkCore;

using Supermarket.Common.Helpers;
using Supermarket.Domain.Entities;

namespace Supermarket.DataAccess.Context
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<SalesInformation> SalesInformation { get; set; }
        public DbSet<Basket> Baskets { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<ProductBasket> ProductBaskets { get; set; }

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

            // Seed Data
            var password = CryptoHelper.Encrypt("Test+-1234*");
            var user = new User
            {
                Id = new Guid("a8ee7c28-e825-48d0-9cca-c2327c5786ea"),
                Name = "Can",
                LastName = "Çalışkan",
                Address = "Karşıyaka",
                CreatedDate = DateTime.Now,
                Email = "cancaliskan@windowslive.com",
                Password = password,
                IsActive = true
            };
            modelBuilder.Entity<User>().HasData(user);
            modelBuilder.Entity<Product>().HasData(new Product()
            {
                Id = new Guid("eb6262bd-bac4-4fac-afcb-34c2774d22c2"),
                IsActive = true,
                CreatedDate = DateTime.Now,
                Description = "Test Product",
                Name = "Product Name",
                Type = "Phone",
                Stock = 5,
                UnitPrice = 99
            });

            base.OnModelCreating(modelBuilder);
        }
    }
}