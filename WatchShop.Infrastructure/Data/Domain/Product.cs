using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WatchShop.Infrastructure.Data.Domain
{
    public class Product
    {
        [Required]
        public int ProductId { get; set; }

        [Required]
        public string ProductName {  get; set; }
        public int CategoryId { get; set; }

        [Required]
        public Category Category {  get; set; }

        [Required]
        public string ProductDescription { get; set; }= string.Empty;

        public int ManufacturerId {  get; set; }
        [Required]
        public Manufacturer Manufacturer {  get; set; }

        [Required]
        public string ProductImage {  get; set; }

        [Required]
        public decimal ProductPrice { get; set; }

        [Required]
        public int Quantity { get; set; }

        [Required]
        public decimal ProductDiscount {  get; set; }


    }
}
