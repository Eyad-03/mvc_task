namespace task1.Models
{
    public class SupplyRequestViewModel
    {
        public List<SupplyItem> Items { get; set; }

        public decimal Subtotal { get; set; }
        public decimal PreparationFee { get; set; }
        public decimal Discount { get; set; }
        public decimal FinalTotal { get; set; }
    }
}