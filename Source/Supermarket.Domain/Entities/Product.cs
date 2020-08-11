using System;
using System.Collections.Generic;

namespace Supermarket.Domain.Entities
{
    public sealed class Product : BaseEntity
    {
        public IList<ProductBasket> ProductBaskets { get; set; }
    }
}