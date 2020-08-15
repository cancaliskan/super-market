using System.Collections.Generic;

using Supermarket.Domain.Entities;

namespace Supermarket.DataAccess.Contracts
{
    public interface IProductRepository : IRepository<Product>
    {
        List<Product> GetRecentlyAddedProducts();
    }
}