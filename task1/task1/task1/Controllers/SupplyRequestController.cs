using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using task1.Models;

namespace task1.Controllers
{
    public class SupplyRequestController : Controller
    {

        private const string SessionKey = "SupplyRequest";

        private static List<SupplyRequest> requests = new List<SupplyRequest>();

        private static int nextRequestId = 1;

        public IActionResult Index()
        {
            string preferredPriority =
                Request.Cookies["PreferredPriority"];

            ViewBag.PreferredPriority = preferredPriority;

            return View();
        }

        [HttpPost]
        public IActionResult AddItem(
            string ProductName,
            string Category,
            int Quantity,
            decimal UnitPrice,
            string Priority)
        {
            if (string.IsNullOrWhiteSpace(ProductName) ||
                Quantity <= 0 || UnitPrice <= 0)
            {
                return BadRequest("Invalid product data.");
            }

            // Retrieve existing products from Session
            var items = GetItems();

            // Add new product
            items.Add(new SupplyItem
            {
                ProductName = ProductName,
                Quantity = Quantity,
                UnitPrice = UnitPrice,
                Priority = Priority
            });

            // Save updated list in Session
            HttpContext.Session.SetString(
                SessionKey,
                JsonSerializer.Serialize(items));

            return RedirectToAction("Index");
        }

        public IActionResult Review()
        {
            // Retrieve products from Session
            var items = GetItems();

            if (items.Count == 0)
            {
                return RedirectToAction("Index");
            }

            // Calculate subtotal
            decimal subtotal = items.Sum(x => x.TotalCost);

            // Preparation Fee
            decimal preparationFee = 0;

            switch (items[0].Priority)
            {
                case "Normal":
                    preparationFee = 2;
                    break;

                case "Urgent":
                    preparationFee = 5;
                    break;

                case "Emergency":
                    preparationFee = 10;
                    break;
            }

            // Discount
            decimal discount = 0;

            if (subtotal > 200)
                discount = subtotal * 0.15m;
            else if (subtotal > 100)
                discount = subtotal * 0.10m;

            // Final Total
            decimal finalTotal =
                subtotal + preparationFee - discount;

            var model = new SupplyRequestViewModel
            {
                Items = items,
                Subtotal = subtotal,
                PreparationFee = preparationFee,
                Discount = discount,
                FinalTotal = finalTotal
            };

            return View(model);
        }


        private List<SupplyItem> GetItems()
        {
            var json = HttpContext.Session.GetString(SessionKey);

            if (string.IsNullOrEmpty(json))
                return new List<SupplyItem>();

            return JsonSerializer.Deserialize<List<SupplyItem>>(json)
                   ?? new List<SupplyItem>();
        }



        [HttpPost]
        public IActionResult ConfirmRequest()
        {
            var items = GetItems();

            if (items.Count == 0)
                return RedirectToAction("Index");

            // Calculate Subtotal
            decimal subtotal = items.Sum(x => x.TotalCost);

            // Calculate Preparation Fee
            decimal preparationFee = 0;

            switch (items[0].Priority)
            {
                case "Normal":
                    preparationFee = 2;
                    break;

                case "Urgent":
                    preparationFee = 5;
                    break;

                case "Emergency":
                    preparationFee = 10;
                    break;
            }

            // Calculate Discount
            decimal discount = 0;

            if (subtotal > 200)
                discount = subtotal * 0.15m;
            else if (subtotal > 100)
                discount = subtotal * 0.10m;

            // Calculate Final Total
            decimal finalTotal = subtotal + preparationFee - discount;

            // Create confirmed request
            var request = new SupplyRequest
            {
                RequestId = nextRequestId++,
                NumberOfProducts = items.Count,
                Subtotal = subtotal,
                PreparationFee = preparationFee,
                Discount = discount,
                FinalTotal = finalTotal,
                Status = "Confirmed"
            };

            // Add request to List
            requests.Add(request);

            // Clear Session after confirmation
            HttpContext.Session.Remove(SessionKey);

            return RedirectToAction("MyRequests");
        }


        [HttpPost]
        public IActionResult ClearDraft()
        {
            HttpContext.Session.Remove(SessionKey);

            return RedirectToAction("Index");
        }


        [HttpPost]
        public IActionResult SavePreference(string Priority)
        {
            CookieOptions options = new CookieOptions
            {
                Expires = DateTime.Now.AddDays(7)
            };

            Response.Cookies.Append(
                "PreferredPriority",
                Priority,
                options
            );

            return RedirectToAction("Index");
        }


        public IActionResult MyRequests()
        {
            return View(requests);
        }


        public IActionResult About()
        {
            return View();
        }


    }
}
