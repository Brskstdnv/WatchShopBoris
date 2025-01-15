using IdentityModel;

using Microsoft.AspNetCore.Identity;

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WatchShop.Infrastructure.Data.Domain
{
    public class ApplicationUser : IdentityUser
    {


        [Required]
        public string FirstName { get; set; } = null!;

        [Required]
        public string LastName { get; set; } = null!;

        [Required]
        public string Adress {  get; set; } = null!;



    }
}
