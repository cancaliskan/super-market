using System;
using System.Collections.Generic;
using Supermarket.Common.Contracts;
using Supermarket.Domain.Entities;

namespace Supermarket.Business.Contracts
{
    public interface IBasketService
    {
        Response<List<Product>> GetDetail(User user);
        Response<bool> Remove(User user, Guid productId);
    }
}