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

        public ContractorVM() 
        { 
            Orders = new List<OrdersVM>();
            Quotes = new List<QuoteVM>();   
            Services = new List<int>();
        
        }
        public List<OrdersVM>? Orders { get; set; }
    }
}
