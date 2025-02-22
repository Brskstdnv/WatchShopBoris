using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;
using WatchShopApp.Core.Service;
using WatchShopApp.Models.Category;
using WatchShopApp.Models.Manufacturer;

namespace WatchShopApp.Models.Product
{
    public class ProductEditVM
    {

        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(30)]
        [Display(Name = "Product Name")]
        public string ProductName { get; set; } = null!;


        [Required]
        [Display(Name = "Manufacturer")]
        public int ManufacturerId { get; set; }
        public virtual List<ManufactrerPairVM> Manufactrers { get; set; } = new List<ManufactrerPairVM>();

        [Required]
        [Display(Name = "Category Name")]
        public int CategoryId { get; set; }
        public virtual List<CategoryPairVM> Categories { get; set; } = new List<CategoryPairVM>();


        [Display(Name = "Picture")]
        public string PictureUrl { get; set; }

        [Required]
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
