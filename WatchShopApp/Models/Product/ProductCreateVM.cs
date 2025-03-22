using Humanizer.Localisation.TimeToClockNotation;

using System.ComponentModel.DataAnnotations;

using WatchShop.Infrastructure.Data.Domain;

using WatchShopApp.Models.Category;
using WatchShopApp.Models.Manufacturer;

namespace WatchShopApp.Models.Product
{
    public class ProductCreateVM
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(30)]
        [Display(Name = "Product Name")]
        public string ProductName { get; set; } = null!;


        [Required]
        [Display(Name = "Manufacturer")]
        public int ManufacturerId {  get; set; }
        public virtual List<ManufactrerPairVM> Manufacturers { get; set; } = new List<ManufactrerPairVM>();


        [Required]
        [Display(Name = "Category")]
        public int CategoryId { get; set; }
        public virtual List<CategoryPairVM> Categories { get; set; } = new List<CategoryPairVM>();

        [Display(Name = "Picture")]
        public string PictureUrl { get; set; } = null!;

        [Display(Name = "Description")]
        public string Description { get; set; } = null!;

        [Range(0, 5000)]
        [Display(Name = "Quantity")]
        public int Quantity { get; set; } = 1;

        [Display(Name = "Price")]
        public decimal Price { get; set; }

        [Display(Name ="Discount")]
        public decimal Discount {  get; set; }





    }
}
