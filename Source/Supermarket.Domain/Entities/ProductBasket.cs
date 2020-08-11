using System;

namespace Supermarket.Domain.Entities
{
    public sealed class ProductBasket : BaseEntity
    {
        public Guid ProductId { get; set; }
        public Product Product { get; set; }

        public Guid BasketId { get; set; }
        public Basket Basket { get; set; }
    }
}