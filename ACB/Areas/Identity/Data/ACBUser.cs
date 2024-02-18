using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace ACB.Areas.Identity.Data;

// Add profile data for application users by adding properties to the ACBUser class
public class ACBUser : IdentityUser
{
    [PersonalData]
    public string FirstName { get; set; }
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
    public string Country { get; set; }
    [PersonalData]
    public string Phone { get; set; }
    [PersonalData]
    public string Company { get; set; }
    [PersonalData]
    public string Title { get; set; }    
}

