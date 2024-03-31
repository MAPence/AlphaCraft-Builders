using System.ComponentModel.DataAnnotations;

namespace ACB.Models
{
    public class ContractorVM
    {
        public int? Id { get; set; }
        [Display(Name = "First Name")]
        public string? FirstName { get; set; }
        [Display(Name = "Last Name")]
        public string? LastName { get; set; }
        public string? Email { get; set; }
        public string? Company { get; set; }
        public string? Address { get; set; }
        public string? City { get; set; }
        public string? State { get; set; }
        public int? Zip {  get; set; }
        public List<int>? Services { get; set; }
        public List<QuoteVM>? Quotes { get; set; }
        public NewOrder? NewOrder { get; set; }
        public QuoteVM? Quote { get; set; }
        public LatLong? LatLong { get; set; }
        public ContractorVM()
        {
            Orders = new List<OrdersVM>();
            Quotes = new List<QuoteVM>();
            Services = new List<int>();
            LatLong = new LatLong();
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
