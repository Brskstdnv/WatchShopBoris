using WatchShop.Infrastructure.Data.Domain;

namespace WatchShopApp.Models.ShoppingCart
{
    public class ShoppingCartVM
    {
        public string UserId { get; set; } = null!;
        public string User { get; set; } = null!;
        public DateTime OrderDate { get; set; }

        public List<ShoppingCartItemVM> Items { get; set; } = new();
        public decimal TotalPrice => Items.Sum(i => i.TotalPrice);
    }
}
