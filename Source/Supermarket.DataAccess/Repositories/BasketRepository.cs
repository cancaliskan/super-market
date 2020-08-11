using Microsoft.EntityFrameworkCore;

using Supermarket.DataAccess.Context;
using Supermarket.DataAccess.Contracts;
using Supermarket.Domain.Entities;

namespace Supermarket.DataAccess.Repositories
{
    public class BasketRepository : Repository<Basket>, IBasketRepository
    {
        public BasketRepository(DbContext context) : base(context)
        {
        }

        public ApplicationDbContext ApplicationContext => Context as ApplicationDbContext;
    }
}