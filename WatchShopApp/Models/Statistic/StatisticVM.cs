using Humanizer.Localisation.TimeToClockNotation;
using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace WatchShopApp.Models.Statistic
{
    public class StatisticVM
    {
        [Display(Name ="Count Clients")]
        public int CountClients { get; set; }

        [Display(Name = "Count Products")]
        public int CountProducts { get; set; }

        [Display(Name = "Count Orders")]
        public int CountOrders { get; set; }

        [Display(Name = "Total Sum Price")]
        public decimal SumOrders { get; set; }


    }
}
