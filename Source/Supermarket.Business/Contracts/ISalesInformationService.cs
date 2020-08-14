using System;
using System.Collections.Generic;
using Supermarket.Common.Contracts;
using Supermarket.Domain.Entities;

namespace Supermarket.Business.Contracts
{
    public interface ISalesInformationService
    {
        Response<List<SalesInformation>> Orders(User user);
        Response<SalesInformation> Detail(Guid id);
    }
}