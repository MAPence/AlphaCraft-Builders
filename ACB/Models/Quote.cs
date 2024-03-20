namespace ACB.Models
{
    public class Quote
    {
        public int id { get; set; }
        public string? Client_first_name { get; set; }
        public string? Client_last_name { get; set; }
        public string? Client_email { get; set; }
        public DateTime? Created { get; set; }
        public string? Details { get; set; }
        public string? Address { get; set; }
        public string? Zip { get; set; }

        public string? City { get; set; }
        public string? State { get; set; }
        public int? Service { get; set; }
    }
}