using Microsoft.EntityFrameworkCore;

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
    public class ShoppingCartService : IShoppingCartService
    {
        private readonly ApplicationDbContext _context;

        public ShoppingCartService(ApplicationDbContext context)
        {
            _context = context;
        }



        public List<ShoppingCartItem> GetShoppingCartItems(string userId)
        {
            return _context.ShoppingCarts
                .Where(x => x.UserId == userId)
                .ToList();
        }

        public void AddToCart(ShoppingCartItem item)
        {
            var existingItem = _context.ShoppingCarts
                .SingleOrDefault(x => x.ProductId == item.ProductId && x.UserId == item.UserId);

            if (existingItem != null)
            {
                existingItem.Quantity += item.Quantity;
                _context.ShoppingCarts.Update(existingItem);
            }
            else
            {
                var product = _context.Products.Find(item.ProductId);
                if (product == null) return;

                item.Price = product.Price;
                item.Discount = product.Discount;
                item.OrderDate = DateTime.Now;

                _context.ShoppingCarts.Add(item);
            }

            _context.SaveChanges();
        }

        public void RemoveFromCart(int cartItemId)
        {
            var item = _context.ShoppingCarts.Find(cartItemId);
            if (item != null)
            {
                _context.ShoppingCarts.Remove(item);
                _context.SaveChanges();
            }
        }

        public void ClearCart(string userId)
        {
            var cartItems = _context.ShoppingCarts
                .Where(x => x.UserId == userId)
                .ToList();

            _context.ShoppingCarts.RemoveRange(cartItems);
            _context.SaveChanges();
        }

        public decimal GetTotalPrice(string userId)
        {
            return _context.ShoppingCarts
                .Where(x => x.UserId == userId)
                .Sum(x => x.Quantity * (x.Price - (x.Price * x.Discount / 100)));
        }

        public void UpdateQuantity(int productId, string userId, int quantity)
        {
            var item = _context.ShoppingCarts.FirstOrDefault(x => x.ProductId == productId && x.UserId == userId);
            if (item != null)
            {
                item.Quantity = quantity;
                _context.SaveChanges();
            }
        }

        public void UpdateItemQuantity(string userId, int productId, int quantity)
        {
            var cartItem = _context.ShoppingCarts
                .FirstOrDefault(x => x.UserId == userId && x.ProductId == productId);

            if (cartItem != null)
            {
                cartItem.Quantity = quantity;
                _context.ShoppingCarts.Update(cartItem);
                _context.SaveChanges();
            }
        }
    }
}
