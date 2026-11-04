namespace task1.Models
{
    // A supply request with all its calculated values
    public class SupplyRequest
    {
        public int RequestId { get; set; }
        public List<SupplyItem> Items { get; set; } = new List<SupplyItem>();
        public string Priority { get; set; } = "Normal";
        public decimal Subtotal { get; set; }
        public decimal PreparationFee { get; set; }
        public decimal DiscountPercent { get; set; }
        public decimal Discount { get; set; }
        public decimal FinalTotal { get; set; }
        public string Status { get; set; } = "Draft";
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public int NumberOfProducts => Items.Count;
    }
}
