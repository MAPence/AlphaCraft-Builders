using System.ComponentModel.DataAnnotations;

namespace ACB.Models
{
    public class Location
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Email { get; set; }
        public string? Address { get; set; }
        public string? City { get; set; }
        public string? State { get; set; }
        [Range(10000, 99999)] // Causes number input
        public string? Zip { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public string? WorkNeeded { get; set; }
    }
}
