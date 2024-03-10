using ACB.Areas.Identity.Data;

namespace ACB.Models
{
    public class OrdersVM
    {
        public int? Id { get; set; }
        public int? co_id { get; set; }
        public int? case_id { get; set; }
        public float? total { get; set; }
        public int? service { get; set; }
        public byte[]? image { get; set; }
        public IList<OrdersVM>? Orders { get; set; }
        public ACBUser CurrentUser { get; set; }
        public DateTime? Created { get; set; }
    }
}
