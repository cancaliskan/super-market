using System;
using System.Collections.Generic;

using Supermarket.Domain.Entities;

namespace Supermarket.DataAccess.Contracts
{
    public interface ISalesInformationRepository : IRepository<SalesInformation>
    {
        List<SalesInformation> GetSalesInformationByUser(User user);
        SalesInformation GetSalesInformation(Guid id);
    }
}