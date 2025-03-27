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



        public void AddToCart(ShoppingCartItem item)
        {
            var existingItem = _context.ShoppingCarts.SingleOrDefault(x => x.ProductId == item.ProductId && x.UserId == item.UserId);

            if (existingItem != null)
            {
                existingItem.Quantity += item.Quantity;
                _context.ShoppingCarts.Add(item);
            }
            //else
            //{
            //    _context.ShoppingCarts.Add(item);
            //}

            _context.SaveChanges();
        }

        public void ClearCart(string userId)
        {
            var items = _context.ShoppingCarts.Where(x=>x.UserId == userId).ToList();
            _context.ShoppingCarts.RemoveRange(items);
            _context.SaveChanges();
        }

        public List<ShoppingCartItem> GetShoppingCartItems(string userId)
        {
            return _context.ShoppingCarts
     .Where(x => x.UserId == userId)
     .Include(x => x.Product) 
     .ToList();
        }

        public void RemoveFromCart(ShoppingCartItem item)
        {
            var existingItem = _context.ShoppingCarts.FirstOrDefault(x=>x.UserId == item.UserId && x.ProductId == item.ProductId);

            if(existingItem != null)
            {
                _context.ShoppingCarts.Remove(existingItem);
                _context.SaveChanges();
            }

        }

        public decimal GetTotalPrice(string userId)
        {
            return _context.ShoppingCarts.Where(x => x.UserId == userId)
                .Sum(x => x.Quantity * x.Price);
        }

    }
}
