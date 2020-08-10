using Microsoft.EntityFrameworkCore;

using Supermarket.DataAccess.Context;
using Supermarket.DataAccess.Contracts;
using Supermarket.Domain.Entities;

namespace Supermarket.DataAccess.Repositories
{
    public class ProductBasketRepository : Repository<ProductBasket>, IProductBasketRepository
    {
        public ProductBasketRepository(DbContext context) : base(context)
        {
        }

        public ApplicationDbContext ApplicationContext => Context as ApplicationDbContext;
    }
}