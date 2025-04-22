using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using WatchShop.Infrastructure.Data.Domain;

using WatchShopApp.Core.Contracts;
using WatchShopApp.Data;

namespace WatchShopApp.Core.Service
{
    public class OrderService : IOrderService
    {
        private readonly IProductService _productService;
        private readonly ApplicationDbContext _context;

        public OrderService(ApplicationDbContext context, IProductService productService)
        {
            _context = context;
            _productService = productService;
        }

        public bool CreateOrderFromCart(string userId)
        {
            var cartItems = _context.ShoppingCarts.Where(x => x.UserId == userId).ToList();

            if (!cartItems.Any()) return false; 

            foreach (var cartItem in cartItems)
            {
                var order = new Order
                {
                    OrderDate = DateTime.Now,
                    UserId = userId,
                    ProductId = cartItem.ProductId,
                    Quantity = cartItem.Quantity,
                    Price = cartItem.Price,
                    Discount = cartItem.Discount
                };

                _context.Orders.Add(order);

                var product = _context.Products.FirstOrDefault(p => p.Id == cartItem.ProductId);
                if (product != null)
                {
                    product.Quantity -= order.Quantity;
                    _context.Products.Update(product); 
                }
            }

            _context.ShoppingCarts.RemoveRange(cartItems); 
            return _context.SaveChanges() > 0;
        }

        public Order GetOrderById(int orderId)
        {
            return _context.Orders.SingleOrDefault(o => o.Id == orderId);
        }

        public List<Order> GetOrdersByUser(string userId)
        {
            return _context.Orders.Where(o => o.UserId == userId).OrderByDescending(o => o.OrderDate).ToList();
        }

        public List<Order> Orders()
        {
            return _context.Orders.OrderByDescending(x => x.OrderDate).ToList();
        }

        public bool RemoveById(int orderId)
        {
            var order = _context.Orders.SingleOrDefault(o => o.Id == orderId);
            if (order == null) return false;

            _context.Orders.Remove(order);
            return _context.SaveChanges() > 0;
        }

        public bool Update(int orderId, int productId, string userId, int quantity)
        {
            var order = _context.Orders.SingleOrDefault(o => o.Id == orderId);
            if (order == null) return false;

            order.Quantity = quantity;
            var product = _context.Products.SingleOrDefault(p => p.Id == productId);
            if (product != null)
            {
                order.Price = product.Price;
                order.Discount = product.Discount;
            }

            _context.Orders.Update(order);
            return _context.SaveChanges() > 0;
        }
    }
   
}

