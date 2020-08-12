using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;

using Supermarket.DataAccess.Context;
using Supermarket.DataAccess.Contracts;
using Supermarket.Domain.Entities;

namespace Supermarket.DataAccess.Repositories
{
    public class BasketRepository : Repository<Basket>, IBasketRepository
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
    }
}