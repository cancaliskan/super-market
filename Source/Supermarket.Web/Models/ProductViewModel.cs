namespace Supermarket.Web.Models
{
    public sealed class ProductViewModel : BaseViewModel
    {
        public string Name { get; set; }
        public string Type { get; set; }
        public int Stock { get; set; }
        public decimal UnitPrice { get; set; }
        public string Description { get; set; }
        public string Image { get; set; }
        public decimal TotalPrice { get; set; }
        public int Count { get; set; }
    }
}