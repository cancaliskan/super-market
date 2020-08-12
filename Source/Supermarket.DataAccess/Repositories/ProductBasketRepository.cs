using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;

using Supermarket.DataAccess.Context;
using Supermarket.DataAccess.Contracts;
using Supermarket.Domain.Entities;

namespace Supermarket.DataAccess.Repositories
{
    public class ProductBasketRepository : Repository<ProductBasket>, IProductBasketRepository
    {
        public ApplicationDbContext ApplicationContext => Context as ApplicationDbContext;

        public ProductBasketRepository(DbContext context) : base(context)
        {
        }

        public List<Product> GetProductsByBasket(Basket basket)
        {
            return ApplicationContext.ProductBaskets.Where(x => x.BasketId == basket.Id && x.IsActive).Select(p => p.Product).ToList();
        }

        public ProductBasket Find(Guid basketId, Guid productId)
        {
            return ApplicationContext.ProductBaskets.FirstOrDefault(x => x.BasketId == basketId && x.ProductId == productId && x.IsActive);
        }
    }
}