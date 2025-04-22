using WatchShop.Infrastructure.Data.Domain;

namespace WatchShopApp.Models.ShoppingCart
{
    public class ShoppingCartItemVM
    {
        public int ProductId { get; set; }
        public string Product { get; set; } = null!;
        public string Picture { get; set; } = null!;
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public decimal Discount { get; set; }
        public decimal TotalPrice => (Price * (1 - (Discount / 100))) * Quantity;
    }
}
