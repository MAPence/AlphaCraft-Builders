namespace ACB.Models
{
    public class NewOrder
    {
        public int? Id { get; set; }
        public int? Co_id { get; set; }
        public int? Case_id { get; set; }
        public decimal? Total { get; set; }
        public decimal? Subtotal { get; set; }
        public decimal? SalesTax { get; set; }
        public string? Notes { get; set; }
        public DateTime? Created { get; internal set; }
    }
}