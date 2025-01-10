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
    public class User : IdentityUser
    {
        public int Id {  get; set; }
        
        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required]
        public string Adress {  get; set; }



    }
}
