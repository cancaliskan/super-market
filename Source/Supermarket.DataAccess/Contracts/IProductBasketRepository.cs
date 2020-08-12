using System;
using System.Collections.Generic;
using Supermarket.Domain.Entities;

namespace Supermarket.DataAccess.Contracts
{
    public interface IProductBasketRepository : IRepository<ProductBasket>
    {
        List<Product> GetProductsByBasket(Basket basket);
        ProductBasket Find(Guid basketId, Guid productId);
    }
}