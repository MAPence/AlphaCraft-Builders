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

        [Compare("Password", ErrorMessage = "Passwords don't match.")]
        [Display(Name = "Confirm Password")]
        [DataType(DataType.Password)]
        public string? ConfirmPassword { get; set; }

        [Required]
        [PersonalData]
        public string FirstName { get; set; }
        [Required]
        [PersonalData]
        public string LastName { get; set; }
        [PersonalData]
        public string Address { get; set; }
        [PersonalData]
        public string City { get; set; }
        [PersonalData]
        public string State { get; set; }
        [PersonalData]
        public string Zip { get; set; }
        [PersonalData]
        public string? Country { get; set; }
        [PersonalData]
        public string Phone { get; set; }
        [PersonalData]
        public string Company { get; set; }
        [PersonalData]
        public string? Title { get; set; }
    }
}