using Supermarket.Domain.Entities;

namespace Supermarket.DataAccess.Contracts
{
    public interface IUserRepository : IRepository<User>
    {
        User GetByEmail(string email);
    }
}