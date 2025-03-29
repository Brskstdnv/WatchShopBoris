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
        List<ShoppingCartItem> GetShoppingCartItems(string userId);

        void AddToCart(ShoppingCartItem item);
        void RemoveFromCart(int cartItemId);

        void ClearCart(string userId);

        decimal GetTotalPrice(string userId);


    }
}

