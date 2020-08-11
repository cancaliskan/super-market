using System;
using System.Collections.Generic;

using Supermarket.Common.Contracts;
using Supermarket.Domain.Entities;

namespace Supermarket.Business.Contracts
{
    public interface IUserService
    {
        Response<User> GetById(Guid id);
        Response<User> Login(string email, string password);
        Response<IEnumerable<User>> GetAll();
        Response<User> Add(User entity);
        Response<User> Update(User entity);
        Response<bool> Remove(Guid id);
    }
}