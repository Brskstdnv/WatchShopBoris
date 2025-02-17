using DocuSign.eSign.Model;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using WatchShop.Infrastructure.Data.Domain;

namespace WatchShopApp.Core.Contracts
{
    internal interface IManufacturerService
    {
        List<Manufacturer> GetManufacturer();
        Manufacturer GetManufacturerId(int manufacturerId);
        List<Product> GetProductByManufacturer(int manufacturerId);
    }
}
