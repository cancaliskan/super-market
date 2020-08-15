using System;
using System.Collections.Generic;

namespace Supermarket.Domain.Entities
{
    public sealed class SalesInformation : BaseEntity
    {
        public Guid UserId { get; set; }
        public User User { get; set; }

        public IList<OrderProductInformation> Orders { get; set; }
        public int TotalItem { get; set; }
        public decimal TotalPrice { get; set; }
    }
}