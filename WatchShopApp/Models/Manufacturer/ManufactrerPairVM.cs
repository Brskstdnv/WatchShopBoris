using System.ComponentModel.DataAnnotations;

namespace WatchShopApp.Models.Manufacturer
{
    public class ManufactrerPairVM
    {
        public int Id {  get; set; }

        [Display(Name = "Manufacturer")]
        public string Name { get; set; } = null!;
    }
}
