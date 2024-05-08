using System.ComponentModel.DataAnnotations;

namespace ACB.Models
{
    public class Location
    {
        public int? Id { get; set; }
        public string? Name { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Email { get; set; }
        public string? Address { get; set; }
        public string? City { get; set; }
        public string? State { get; set; }
        [Range(10000, 99999)] // Causes number input
        public string? Zip { get; set; }
        public decimal? Latitude { get; set; }
        public decimal? Longitude { get; set; }
        public string? WorkNeeded { get; set; }
        public int? Distance { get; set; }

        public List<ContractorTile>? Results { get; set; }

        public Location()
        {
            Results = new List<ContractorTile>();   
        }


    }
}