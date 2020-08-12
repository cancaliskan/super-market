using System;
using System.Collections.Generic;
using Supermarket.Common.Contracts;
using Supermarket.Domain.Entities;

namespace Supermarket.Business.Contracts
{
    public interface IProductBasketService
    {
        Response<bool> AddToBasket(User user, Guid productId);
        Response<bool> RemoveFromBasket(User user, Guid productId);
        Response<List<Product>> GetBasketDetails(User user);
    }
}