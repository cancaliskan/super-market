using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;

using Supermarket.DataAccess.Context;
using Supermarket.DataAccess.Contracts;
using Supermarket.Domain.Entities;

namespace Supermarket.DataAccess.Repositories
{
    public sealed class ProductRepository : Repository<Product>, IProductRepository
    {
        public ApplicationDbContext ApplicationContext => Context as ApplicationDbContext;

        public ProductRepository(DbContext context) : base(context)
        {
        }

        /// <summary>
        /// Return added 5 products
        /// </summary>
        /// <returns></returns>
        public List<Product> GetRecentlyAddedProducts()
        {
            return ApplicationContext.Products.Where(x => x.IsActive).OrderByDescending(p => p.CreatedDate).Take(5).ToList();
        }
    }
}