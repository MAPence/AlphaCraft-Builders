namespace ACB.Models
{
    public class ContactSubmission
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string Message { get; set; }
        public DateTime SubmittedAt { get; set; }
    }
}