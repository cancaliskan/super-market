using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;

using Supermarket.DataAccess.Context;
using Supermarket.DataAccess.Contracts;
using Supermarket.Domain.Entities;

namespace Supermarket.DataAccess.Repositories
{
    public sealed class BasketRepository : Repository<Basket>, IBasketRepository
    {
        public ApplicationDbContext ApplicationContext => Context as ApplicationDbContext;

        public BasketRepository(DbContext context) : base(context)
        {
        }

        public Basket GetBasketByUser(User user)
        {
            var basket = ApplicationContext.Baskets.FirstOrDefault(b => b.UserId == user.Id);
            if (basket == null)
            {
                basket = new Basket()
                {
                    Id = Guid.NewGuid(),
                    UserId = user.Id,
                    IsActive = true,
                    CreatedDate = DateTime.Now,
                    User = user
                };
                ApplicationContext.Baskets.Add(basket);
            }

            return basket;
        }

        public bool CompleteOrder(Guid id)
        {
            var basket = ApplicationContext.Baskets.Find(id);
            if (basket == null)
            {
                return false;
            }

            basket.Products = null;
            ApplicationContext.Baskets.Update(basket);

            var products = ApplicationContext.ProductBaskets.Where(x => x.BasketId == id).ToList();
            ApplicationContext.ProductBaskets.RemoveRange(products);

            return true;
        }

        public new Basket GetById(Guid id)
        {
            return ApplicationContext.Baskets.Include("User").Include("Products").FirstOrDefault(x => x.Id == id && x.IsActive);
        }
    }
}