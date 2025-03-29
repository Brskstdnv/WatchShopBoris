using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using WatchShop.Infrastructure.Data.Domain;

namespace WatchShopApp.Core.Contracts
{
    public interface IOrderService
    {
        //bool Create(int productId, string userId, int quantity);

        //List<Order> Orders();

        //public List<Order> GetOrdersByUser(string userId);
        //Order GetOrderById(int orderId);
        //bool RemoveById(int orderId);
        //bool Update(int orderId, int productId, string userId, int quantity);
        bool CreateOrderFromCart(string userId);
        Order GetOrderById(int orderId);
        List<Order> GetOrdersByUser(string userId);
        List<Order> Orders();
        bool RemoveById(int orderId);
        bool Update(int orderId, int productId, string userId, int quantity);





    }
}
