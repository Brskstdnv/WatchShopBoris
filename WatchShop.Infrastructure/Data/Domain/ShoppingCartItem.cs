using Org.BouncyCastle.Asn1.Crmf;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WatchShop.Infrastructure.Data.Domain
{
    public class ShoppingCartItem
    {
        public int Id { get; set; }

        [Required]
        public DateTime OrderDate { get; set; } = DateTime.Now;
        public int ProductId { get; set; }
        public virtual Product Product { get; set; } = null!;
        public string UserId { get; set; }
        public virtual ApplicationUser User { get; set; } = null!;
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public decimal Discount { get; set; }
        public decimal TotalPrice => Quantity * Price - (Quantity * Price * Discount / 100);
    }




}

