using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WatchShop.Infrastructure.Data.Domain
{
    public class Order
    {
        public int Id { get; set; }

        public int UserId {  get; set; }
        public User User { get; set; }

        public int ProductId { get; set; }
        public Product Product {  get; set; }

        public DateTime DateTime { get; set; }

        public int Quantity { get; set; }

        public decimal CurrentPrice {  get; set; }

        public decimal CurrentDiscount {  get; set; }
    }
}
