namespace task1.Models
{
    public class SupplyRequest
    {
        public int RequestId { get; set; }

        public int NumberOfProducts { get; set; }

        public decimal Subtotal { get; set; }

        public decimal PreparationFee { get; set; }

        public decimal Discount { get; set; }

        public decimal FinalTotal { get; set; }

        public string Status { get; set; }
    }
}