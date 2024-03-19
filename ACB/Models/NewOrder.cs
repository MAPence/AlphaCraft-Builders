namespace ACB.Models
{
    public class NewOrder
    {
        public int? Id { get; set; }
        public int? Co_id { get; set; }
        public int? Case_id { get; set; }
        public float? Total { get; set; }
        public DateTime? Created { get; internal set; }
    }
}