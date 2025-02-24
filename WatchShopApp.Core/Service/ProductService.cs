using Microsoft.EntityFrameworkCore;

using Org.BouncyCastle.Crypto.Prng;

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
    public class ProductService : IProductService
    {
        public ApplicationDbContext _context;

        public ProductService(ApplicationDbContext context)
        {
            _context = context;
        }
        public bool Create(string name, int categoryId, int manufacturerId, string picture, string description, int quantity, decimal price, decimal discount)
        {
            Product item = new Product()
            {
                ProductName = name,
                Manufacturer = _context.Manufacturers.Find(manufacturerId),
                Category = _context.Categories.Find(categoryId),

                Picture = picture,
                Quantity = quantity,
                Price = price,
                Description = description,
                Discount = discount


            };
            _context.Products.Add(item);
            return _context.SaveChanges() != 0;
        }

        public List<Product> GetAllProducts()
        {
            List<Product> products = _context.Products.ToList();
            return products;
        }

        public Product GetProductById(int productId)
        {
            return _context.Products.Find(productId);
        }

        public List<Product> GetProducts(string searchStringCategoryName, string searchStringManufacturerName)
        {
            List<Product> products = _context.Products.ToList();
            if (!String.IsNullOrEmpty(searchStringManufacturerName) && !String.IsNullOrEmpty(searchStringManufacturerName))
            {
                products = products.Where(x => x.Category.CategoryName.ToLower().Contains(searchStringCategoryName.ToLower())
                && x.Manufacturer.Name.ToLower().Contains(searchStringManufacturerName.ToLower())).ToList();
            }
            else if (!String.IsNullOrEmpty(searchStringCategoryName))
            {
                products = products.Where(x => x.Category.CategoryName.ToLower().Contains(searchStringCategoryName.ToLower())).ToList();
            }
            else if (!String.IsNullOrEmpty(searchStringManufacturerName))
            {
                products = products.Where(x => x.Manufacturer.Name.ToLower().Contains(searchStringManufacturerName.ToLower())).ToList();
            }
            return products;
        }

        public bool RemoveById(int productId)
        {
            var product = GetProductById(productId);
            if(product == default(Product))
            {
                return false;
            }
            _context.Products.Remove(product);
            return _context.SaveChanges() != 0;
        }

        public bool Update(int productId, string name, int manufacturerId, int categoryId, string picture, string description, int quantity, decimal price, decimal discount)
        {
            var product = GetProductById(productId);
            if(product == default(Product))
            {
                return false;
            }
            product.ProductName = name;


            product.Manufacturer = _context.Manufacturers.Find(manufacturerId);
            product.Category = _context.Categories.Find(categoryId);

            product.Picture = picture;
            product.Quantity = quantity;
            product.Price = price;
            product.Discount = discount;
            product.Description = description;
            _context.Update(product);
            return _context.SaveChanges() != 0;
        }
    }
}
