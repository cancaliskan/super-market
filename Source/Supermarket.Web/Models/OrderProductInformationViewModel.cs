using System;

namespace Supermarket.Web.Models
{
    public class OrderProductInformationViewModel : BaseViewModel
    {
        public Guid ProductId { get; set; }
        public ProductViewModel Product { get; set; }

        public decimal UnitPrice { get; set; }
        public decimal TotalPrice { get; set; }
        public int Count { get; set; }
    }
}