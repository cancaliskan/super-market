using System;
using System.Collections.Generic;

namespace Supermarket.Web.Models
{
    public sealed class BasketViewModel : BaseViewModel
    {
        public List<ProductDetail> Products { get; set; }
        public decimal TotalPrice { get; set; }
        public int ProductCount { get; set; }
    }

    public sealed class ProductDetail
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public decimal UnitPrice { get; set; }
        public int Count { get; set; }
        public decimal Total { get; set; }
    }
}