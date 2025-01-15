using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WatchShop.Infrastructure.Data.Domain
{
    public class Category
    {
        public int Id { get; set; }

        [Required]
        public string CategoryName {  get; set; } = null!;

        public virtual IEnumerable<Product> Products { get; set; } = new List<Product>();


    }
}
