using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using WatchShop.Infrastructure.Data.Domain;

namespace WatchShopApp.Core.Contracts
{
    public interface IProductService
    {
        bool Create(string name, int categoryId, string category, int manufacturerId, string manufacturer, string picture, int quantity, decimal price, decimal discount);

        bool Update(int productId, string name, int categoryId, string category, string picture, int quantity, decimal price, decimal discount);

        List<Product> GetAllProducts();

        Product GetProductById(int productId);

        bool RemoveById(int productId);

        List<Product> GetProducts(string searchStringCategoryName, string searchStringManufacturerName);
    }
}
