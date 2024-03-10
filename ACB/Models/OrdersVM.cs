using ACB.Areas.Identity.Data;

namespace ACB.Models
{
    public class OrdersVM
    {
        public int? Id { get; set; }
        public int? Co_id { get; set; }
        public int? Case_id { get; set; }
        public float? Total { get; set; }
        public int? Service { get; set; }
        public byte[]? Image { get; set; }
        public IList<OrdersVM>? Orders { get; set; }
        public ACBUser CurrentUser { get; set; }
        public DateTime? Created { get; set; }
    }
}