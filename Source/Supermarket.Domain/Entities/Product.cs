using System.Collections.Generic;

namespace Supermarket.Domain.Entities
{
    public sealed class Product : BaseEntity
    {
        public string Name { get; set; }
        public string Type { get; set; }
        public int Stock { get; set; }
        public decimal UnitPrice { get; set; }
        public string Description { get; set; }

        public IList<OrderProductInformation> Orders { get; set; }
    }
}