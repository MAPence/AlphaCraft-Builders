using System.ComponentModel.DataAnnotations;

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
        [Range(10000, 99999)] // Causes number input
        public string zip { get; set; }

        public string? city { get; set; }
        public string? state { get; set; }
        public int? service { get; set; }
    }
}
