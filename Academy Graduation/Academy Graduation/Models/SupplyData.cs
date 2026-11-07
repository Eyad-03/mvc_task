// Models/SupplyData.cs
using Academy_Graduation.Models;


    public static class SupplyData
    {
        public static List<Category> Categories = new()
        {
            new Category { Id = 1, Name = "Graduation Certificates", Suppliers = new()
            {
                new Supplier { Id = 1, Name = "Supplier A", Offer = new Offer { Id = 1, Name = "Premium Certificate Package",  Quantity = 35, UnitPrice = 2m } },
                new Supplier { Id = 2, Name = "Supplier B", Offer = new Offer { Id = 2, Name = "Standard Certificate Package", Quantity = 35, UnitPrice = 1.5m } },
                new Supplier { Id = 3, Name = "Supplier C", Offer = new Offer { Id = 3, Name = "Luxury Certificate Package",   Quantity = 35, UnitPrice = 2.5m } }
            }},
            new Category { Id = 2, Name = "Hospitality", Suppliers = new()
            {
                new Supplier { Id = 4, Name = "Supplier D", Offer = new Offer { Id = 4, Name = "Coffee & Drinks Package",     Quantity = 100, UnitPrice = 2m } },
                new Supplier { Id = 5, Name = "Supplier E", Offer = new Offer { Id = 5, Name = "Sandwiches & Snacks Package", Quantity = 100, UnitPrice = 3m } },
                new Supplier { Id = 6, Name = "Supplier F", Offer = new Offer { Id = 6, Name = "Full Hospitality Package",    Quantity = 100, UnitPrice = 4m } }
            }},
            new Category { Id = 3, Name = "Graduation Gifts", Suppliers = new()
            {
                new Supplier { Id = 7, Name = "Supplier G", Offer = new Offer { Id = 7, Name = "Customized Mugs Package",   Quantity = 35, UnitPrice = 2m } },
                new Supplier { Id = 8, Name = "Supplier H", Offer = new Offer { Id = 8, Name = "Graduation Bags Package",   Quantity = 35, UnitPrice = 3m } },
                new Supplier { Id = 9, Name = "Supplier I", Offer = new Offer { Id = 9, Name = "Premium Gift Boxes Package", Quantity = 35, UnitPrice = 4m } }
            }}
        };
    }
