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

        public QuoteVM? Quote { get; set; }

        public ContractorVM() 
        { 

            Quotes = new List<QuoteVM>();   
            Services = new List<int>();
        
        }

    }
}
