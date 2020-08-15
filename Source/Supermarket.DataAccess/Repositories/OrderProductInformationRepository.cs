using Microsoft.EntityFrameworkCore;

using Supermarket.DataAccess.Context;
using Supermarket.DataAccess.Contracts;
using Supermarket.Domain.Entities;

namespace Supermarket.DataAccess.Repositories
{
    public sealed class OrderProductInformationRepository : Repository<OrderProductInformation>, IOrderProductInformationRepository
    {
        public ApplicationDbContext ApplicationContext => Context as ApplicationDbContext;

        public OrderProductInformationRepository(DbContext context) : base(context)
        {
        }
    }
}