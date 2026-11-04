namespace task1.Models
{
    public class SupplyItem
    {
        public string ProductName { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public string Priority { get; set; }

        public decimal TotalCost => Quantity * UnitPrice;
    }
}