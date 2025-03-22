using WatchShop.Infrastructure.Data.Domain;

namespace WatchShopApp.Models.ShoppingCart
{
    public class ShoppingCartVM
    {
        public List<ShoppingCartItem> Items { get; set; } = new List<ShoppingCartItem>();
        public decimal TotalPrice { get; set; }

        public ShoppingCartVM()
        {
            TotalPrice = Items.Sum(item => item.Price * item.Quantity);
        }

    }
}
