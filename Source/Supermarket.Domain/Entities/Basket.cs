using System;
using System.Collections.Generic;

namespace Supermarket.Domain.Entities
{
    public sealed class Basket : BaseEntity
    {
        public Guid UserId { get; set; }
        public User User { get; set; }

        public IList<ProductBasket> ProductBasket { get; set; }
    }
}