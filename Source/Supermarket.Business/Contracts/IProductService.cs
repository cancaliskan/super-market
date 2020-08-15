using System;
using System.Collections.Generic;

using Supermarket.Common.Contracts;
using Supermarket.Domain.Entities;

namespace Supermarket.Business.Contracts
{
    public interface IProductService
    {
        Response<Product> GetById(Guid id);
        Response<List<Product>> GetAll();
        Response<List<Product>> GetRecentlyAddedProducts();
        Response<Product> Add(Product entity);
        Response<Product> Update(Product entity);
        Response<bool> Remove(Product entity);
    }
}