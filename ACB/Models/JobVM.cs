using ACB.Areas.Identity.Data;

namespace ACB.Models
{
    public class JobVM
    {
        public int? Id { get; set; }
        public string? Firstname { get; set; }
        public string? Lastname { get; set; }
        public string? Email { get; set; }
        public string? Service { get; set; }
        public string? Address { get; set; }
        public string? City { get; set; }
        public string? State { get; set; }
        public int? Zip { get; set; }
        public string? Details { get; set; }
        public DateTime? Start { get; set; }
        public DateTime? End { get; set; }
        public Decimal? Amount { get; set; }
        public IList<JobVM>? Jobs { get; set; }
        public ACBUser CurrentUser { get; set; }
    }
}