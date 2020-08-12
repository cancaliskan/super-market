using System;
using Supermarket.Common.Contracts;
using Supermarket.Domain.Entities;

namespace Supermarket.Business.Contracts
{
    public interface IProductBasketService
    {
        Response<bool> AddToBasket(User user, Guid productId);
    }
}