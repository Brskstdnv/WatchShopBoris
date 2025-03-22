using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WatchShop.Infrastructure.Data.Domain;

namespace WatchShopApp.Core.Contracts
{
    public interface IShoppingCartService
    {
        public List<ShoppingCartItem> GetShoppingCartItems(string userId);

        public void AddToCart(ShoppingCartItem item);

        public void RemoveFromCart(ShoppingCartItem item);

        public void ClearCart(string userId);

        decimal GetTotalPrice(string userId);
    }


}

