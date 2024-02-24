using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using ACB.Areas.Identity.Data;
using Microsoft.AspNetCore.Identity;



// Add profile data for application users by adding properties to the ACBUser class




namespace ACB.Models
{
    public class User 
    {

        [DataType(DataType.EmailAddress)]
        public string? Email { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string? Password { get; set; }

        public ACBUser Info { get; set; }
    }
}
