using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace WatchShopApp.Models.Product
{
    public class ProductDeleteVM
    {
        [Key]
        public int Id { get; set; }

        [Display(Name = "ProductName")]
        public string ProductName { get; set; }

        public int ManufactureId { get; set; }
        [Display(Name = "Manufacture")]
        public string ManufactureName { get; set; } = null!;

        public int CategoryId { get; set; }
        [Display(Name = "Category")]
        public string CategoryName { get; set; } = null!;

        [Display(Name = "Picture")]
        public string PictureUrl { get; set; } = null!;

        [Display(Name = "Description")]
        public string Description { get; set; }

        [Display(Name = "Quantity")]
        public int Quantity { get; set; }

        [Display(Name = "Price")]
        public decimal Price { get; set; }

        [Display(Name = "Discount")]
        public decimal Discount { get; set; }
    }
}
