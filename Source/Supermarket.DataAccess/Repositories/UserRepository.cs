using System.Linq;
using Microsoft.EntityFrameworkCore;

using Supermarket.DataAccess.Context;
using Supermarket.DataAccess.Contracts;
using Supermarket.Domain.Entities;

namespace Supermarket.DataAccess.Repositories
{
    public class UserRepository : Repository<User>, IUserRepository
    {
        public ApplicationDbContext ApplicationContext => Context as ApplicationDbContext;

        public UserRepository(DbContext context) : base(context)
        {
        }

        public User GetByEmail(string email)
        {
            return ApplicationContext.Users.FirstOrDefault(x => x.Email == email && x.IsActive);
        }
    }
}