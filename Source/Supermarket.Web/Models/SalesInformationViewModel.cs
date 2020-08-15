using System.Collections.Generic;

namespace Supermarket.Web.Models
{
    public sealed class SalesInformationViewModel : BaseViewModel
    {
        public IList<OrderProductInformationViewModel> Orders { get; set; }
        public int TotalItem { get; set; }
        public decimal TotalPrice { get; set; }
    }
}