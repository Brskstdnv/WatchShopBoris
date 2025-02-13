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
    public class ProductService // IProductService
    {
        //public ApplicationDbContext _context;

        //public ProductService(ApplicationDbContext context)
        //{
        //    _context = context;
        //}
        //public bool Create(string name, int categoryId,  int manufacturerId,  string picture, int quantity, decimal price, decimal discount)
        //{
        //    Product product = new Product()
        //    {
        //        ProductName = name,
        //        Manufacture = _context.Manufacturers.Find(manufacturerId),
        //        Category = _context.Categories.Find(categoryId),

        //        ProductImage = picture,
        //        Quantity = quantity,
        //        ProductPrice = price,
        //        ProductDiscount = discount


        //    };
        //    _context.Products.Add(product);
        //   return _context.SaveChanges() != 0;
        //}

        //public List<Product> GetAllProducts()
        //{
        //    List<Product> products = _context.Products.ToList();
        //    return products;
        //}

        //public Product GetProductById(int productId)
        //{
        //    return _context.Products.Find(productId);
        //}

        //public List<Product> GetProducts(string searchStringCategoryName, string searchStringManufacturerName)
        //{
        //    List<Product> products = _context.Products.ToList();
        //    if(!String.IsNullOrEmpty(searchStringManufacturerName))
        //}

        //public bool RemoveById(int productId)
        //{
        //    throw new NotImplementedException();
        //}

        //public bool Update(int productId, string name, int categoryId, string category, string picture, int quantity, decimal price, decimal discount)
        //{
        //    throw new NotImplementedException();
        //}
    }
}
