using Microsoft.EntityFrameworkCore;
using Supermarket.DataAccess.Context;
using Supermarket.DataAccess.Contracts;
using Supermarket.Domain.Entities;

namespace Supermarket.DataAccess.Repositories
{
    public class SalesInformationRepository : Repository<SalesInformation>, ISalesInformationRepository
    {
        public SalesInformationRepository(DbContext context) : base(context)
        {
        }

        public ApplicationDbContext ApplicationContext => Context as ApplicationDbContext;
    }
}