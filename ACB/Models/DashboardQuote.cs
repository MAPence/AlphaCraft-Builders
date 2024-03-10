namespace ACB.Models
{
    public class DashboardQuote
    {
        //Displays a list of Quotes on the Dashboard
        public int Id { get; set; }
        public string? Client_first_name { get; set; }
        public string? Client_last_name { get; set; }
        public string? Client_email { get; set; }
        public DateTime? Created { get; set; }
        public string? Details { get; set; }
        public string? Address { get; set; }
        public int Zip { get; set; }
        public string? City { get; set; }
        public string? State { get; set; }
        public int? Service { get; set; }
        // Usage to show list
        public IList<Quote> Quotes { get; set; }
    }
}