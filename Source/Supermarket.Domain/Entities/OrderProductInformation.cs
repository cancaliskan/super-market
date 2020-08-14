using System;

namespace Supermarket.Domain.Entities
{
    public sealed class OrderProductInformation : BaseEntity
    {
        public Guid ProductId { get; set; }
        public Product Product { get; set; }

        public Guid SalesInformationId { get; set; }
        public SalesInformation SalesInformation{ get; set; }

        public decimal UnitPrice { get; set; }
        public decimal TotalPrice { get; set; }
        public int Count { get; set; }
    }
}