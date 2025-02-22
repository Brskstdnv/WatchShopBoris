using System.ComponentModel.DataAnnotations;

namespace WatchShopApp.Models.Product
{
    public class ProductDetailsVM
    {
        [Key]
        public int Id { get; set; }

        [Display(Name = "ProductName")]
        public string ProductName { get; set; }

        public int ManufactureId { get; set; }
        [Display(Name = "Manufacture")]
        public string ManufactureName { get; set; }

        public int CategoryId { get; set; }
        [Display(Name = "Category")]
        public string CategoryName { get; set; }

        [Display(Name = "Picture")]
        public string PictureUrl { get; set; }

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
