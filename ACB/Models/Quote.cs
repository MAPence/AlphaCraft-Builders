using System.ComponentModel.DataAnnotations;

namespace ACB.Models
{
    public class Quote
    {
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
        public byte[]? Image { get; set; }

        public double? Longitude { get; set; }
        public double? Latitude { get; set;}
    }
}