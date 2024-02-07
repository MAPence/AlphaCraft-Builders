namespace ACB.Models
{
    public class Client
    {
        public int id { get; set; }
        public string? client_first_name { get; set; }
        public string? client_last_name { get; set; }
        public string? client_email { get; set; }
        public DateTime? created { get; set; }
        public string? address { get; set; }
        public int? client_phone { get; set; }
    }
}
