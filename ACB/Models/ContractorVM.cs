using System.ComponentModel.DataAnnotations;

namespace ACB.Models
{
    public class ContractorVM
    {
        public int? Id { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set;}
        public string? Email { get; set; }
        public List<int>? Services { get; set; }
        public List<QuoteVM>? Quotes { get; set; }
        public NewOrder? NewOrder { get; set; }
        public QuoteVM? Quote { get; set; }
        public ContractorVM() 
        { 
            Orders = new List<OrdersVM>();
            Quotes = new List<QuoteVM>();   
            Services = new List<int>();
        }
        public List<OrdersVM>? Orders { get; set; }

        //User Password Settings
        [DataType(DataType.Password)]
        [Display(Name = "Current Password")]
        public string CurrentPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "New Password")]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm New Password")]
        [Compare("NewPassword", ErrorMessage = "The new password and confirmation password do not match.")]
        public string ConfirmNewPassword { get; set; }
    }
}
