namespace task1.Models
{
    // One product line inside a supply request (bound from the Index form via Model Binding)
    public class SupplyItem
    {
        public string ProductName { get; set; } = string.Empty;
        public string Category { get; set; } = string.Empty;
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public string Priority { get; set; } = "Normal";

        // Product total cost = Quantity x Unit Price
        public decimal TotalCost => Quantity * UnitPrice;
    }
}
