using System.Collections.Generic;

using Supermarket.Common.Contracts;
using Supermarket.Domain.Entities;

namespace Supermarket.Business.Contracts
{
    public interface IUserService
    {
        Response<User> GetById(int id);
        Response<IEnumerable<User>> GetAll();
        Response<User> Add(User entity);
        Response<User> Update(User entity);
        Response<bool> Remove(int id);
    }
}