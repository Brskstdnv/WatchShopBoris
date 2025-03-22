using System.ComponentModel.DataAnnotations;

namespace WatchShopApp.Models.Order
{
    public class OrderCreateVM
    {
        public string Id { get; set; }

        public DateTime OrderDate {  get; set; }

        public int ProductId {  get; set; }
        public string ProductName { get; set; } = null!;
        public int QuantityInStock {  get; set; }

        public string Picture {  get; set; } = null!;

        //public string Description { get; set; } = null!;

        [Range(1, 100)]
        public int Quantity { get; set; } = 1;
        public decimal Price {  get; set; }
        public decimal Discount {  get; set; }

        public decimal TotalPrice {  get; set; }
    }
}
