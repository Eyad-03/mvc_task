namespace Academy_Graduation.Models
{
    public class ReviewItem
    {
        public string Category { get; set; } = "";
        public string Supplier { get; set; } = "";
        public string Offer { get; set; } = "";
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal Total { get; set; }
    }
}
