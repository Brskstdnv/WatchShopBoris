using DocuSign.eSign.Model;

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
    public class ManufacturerIdService : IManufacturerService
    {

        private readonly ApplicationDbContext _context;

        public ManufacturerIdService(ApplicationDbContext context)
        {
            _context = context;
        }

        public List<Manufacturer> GetManufacturer()
        {
            List<Manufacturer> manufacturers = new List<Manufacturer>();
            return manufacturers;
        }

        public Manufacturer GetManufacturerId(int manufacturerId)
        {
            return _context.Category.Find(manufacturerId);
        }

        public List<Product> GetProductByManufacturer(int manufacturerId)
        {
            return _context.Products
                .Where(x => x.ManufacturerdId == manufacturerId).ToList();
        }

       
    }
}
