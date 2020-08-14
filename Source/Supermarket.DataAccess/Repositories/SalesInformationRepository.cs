using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Supermarket.DataAccess.Context;
using Supermarket.DataAccess.Contracts;
using Supermarket.Domain.Entities;

namespace Supermarket.DataAccess.Repositories
{
    public class SalesInformationRepository : Repository<SalesInformation>, ISalesInformationRepository
    {
        public ApplicationDbContext ApplicationContext => Context as ApplicationDbContext;

        public SalesInformationRepository(DbContext context) : base(context)
        {
        }

        public List<SalesInformation> GetSalesInformationByUser(User user)
        {
            return ApplicationContext.SalesInformation.Include(x => x.Orders).Where(b => b.UserId == user.Id).ToList();
        }

        public SalesInformation GetSalesInformation(Guid id)
        {
            return ApplicationContext.SalesInformation.Include(x => x.Orders)
                                                      .ThenInclude(x=>x.Product)
                                                      .FirstOrDefault(s => s.Id == id);
        }
    }
}