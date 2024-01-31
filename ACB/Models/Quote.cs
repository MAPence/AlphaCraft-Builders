namespace ACB.Models
{
    public class Quote
    {
        public int id { get; set; }
        public string? client_first_name { get; set; }
        public string? client_last_name { get; set; }
        public string? client_email { get; set; }
        public DateTime? created { get; set; }
        public string? details { get; set; }
        public string? address { get; set; }
        public int zip { get; set; }


    }
}
