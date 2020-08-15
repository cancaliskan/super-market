using System;

using Supermarket.Common.Contracts;
using Supermarket.Domain.Entities;

namespace Supermarket.Business.Contracts
{
    public interface IUserService
    {
        Response<User> GetById(Guid id);
        Response<User> Login(string email, string password);
        Response<User> Add(User entity);
        Response<User> GetUserByEmail(string eMail);
    }
}