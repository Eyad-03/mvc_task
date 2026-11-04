using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using task1.Data;
using task1.Models;

namespace task1.Controllers
{
    public class SupplyRequestController : Controller
    {
        // Session key used to store the current draft request
        private const string DraftSessionKey = "DraftRequest";

        // Cookie name used to remember the employee's preferred priority
        private const string PreferenceCookieName = "PreferredPriority";

        private static readonly string[] Priorities = { "Normal", "Urgent", "Emergency" };
        private static readonly string[] Categories = { "Hot Drinks", "Cold Drinks", "Dairy", "Food", "Snacks", "Water", "Packaging" };

        // ---------------------------------------------------------------
        // GET: /SupplyRequest  -> Create Supply Request page
        // ---------------------------------------------------------------
        [HttpGet]
        public IActionResult Index()
        {
            string preferred = GetPreferredPriority();

            // Pre-select the saved preferred priority in the form
            var newItem = new SupplyItem { Priority = preferred };

            FillIndexViewBag(preferred);
            return View(newItem);
        }

        // ---------------------------------------------------------------
        // POST: /SupplyRequest/AddItem  -> adds one product to the draft
        // ---------------------------------------------------------------
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AddItem(SupplyItem item)   // Model Binding
        {
            // ---- Validation ----
            if (string.IsNullOrWhiteSpace(item.ProductName))
            {
                ModelState.AddModelError(nameof(item.ProductName), "Product name cannot be empty.");
            }

            if (string.IsNullOrWhiteSpace(item.Category))
            {
                ModelState.AddModelError(nameof(item.Category), "Please select a category.");
            }

            if (item.Quantity <= 0)
            {
                ModelState.AddModelError(nameof(item.Quantity), "Quantity must be greater than zero.");
            }

            if (item.UnitPrice <= 0)
            {
                ModelState.AddModelError(nameof(item.UnitPrice), "Unit price must be greater than zero.");
            }

            if (!Priorities.Contains(item.Priority))
            {
                ModelState.AddModelError(nameof(item.Priority), "Please select a valid priority.");
            }

            if (!ModelState.IsValid)
            {
                // Show the form again with the error messages and the entered values
                FillIndexViewBag(GetPreferredPriority());
                return View("Index", item);
            }

            item.ProductName = item.ProductName.Trim();

            // ---- Save the product in the Session draft ----
            List<SupplyItem> draft = GetDraft();
            draft.Add(item);
            SaveDraft(draft);

            TempData["Success"] = $"{item.ProductName} ({item.Quantity} × {item.UnitPrice:0.###} JD) was added to your supply request.";
            return RedirectToAction(nameof(Index));
        }

        // ---------------------------------------------------------------
        // GET: /SupplyRequest/Review  -> shows the draft and the calculations
        // ---------------------------------------------------------------
        [HttpGet]
        public IActionResult Review()
        {
            List<SupplyItem> draft = GetDraft();   // retrieve the saved request from Session

            if (draft.Count == 0)
            {
                ViewBag.IsEmpty = true;
                return View(new SupplyRequest());
            }

            SupplyRequest request = CalculateRequest(draft);
            ViewBag.IsEmpty = false;
            return View(request);
        }

        // ---------------------------------------------------------------
        // POST: /SupplyRequest/ConfirmRequest
        // ---------------------------------------------------------------
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ConfirmRequest()
        {
            List<SupplyItem> draft = GetDraft();

            // Do not allow an empty supply request
            if (draft.Count == 0)
            {
                TempData["Error"] = "You cannot submit an empty supply request. Please add at least one product.";
                return RedirectToAction(nameof(Index));
            }

            SupplyRequest request = CalculateRequest(draft);
            request.Status = "Confirmed";
            request.CreatedAt = DateTime.Now;

            // Store the confirmed request in the application's List
            SupplyRequestStore.Add(request);

            // Clear the draft from Session after a successful confirmation
            HttpContext.Session.Remove(DraftSessionKey);

            TempData["Success"] = $"Supply request #{request.RequestId} was confirmed successfully. Final total: {request.FinalTotal:0.00} JD.";
            return RedirectToAction(nameof(MyRequests));
        }

        // ---------------------------------------------------------------
        // POST: /SupplyRequest/ClearDraft  -> removes all draft data from Session
        // ---------------------------------------------------------------
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ClearDraft()
        {
            HttpContext.Session.Remove(DraftSessionKey);

            TempData["Success"] = "Your draft supply request was cleared.";
            return RedirectToAction(nameof(Index));
        }

        // ---------------------------------------------------------------
        // POST: /SupplyRequest/SavePreference  -> saves preferred priority in a Cookie
        // ---------------------------------------------------------------
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult SavePreference(string preferredPriority)
        {
            if (!Priorities.Contains(preferredPriority))
            {
                TempData["Error"] = "Please select a valid priority (Normal, Urgent or Emergency).";
                return RedirectToAction(nameof(Index));
            }

            var options = new CookieOptions
            {
                Expires = DateTimeOffset.Now.AddDays(7),   // cookie expires after 7 days
                HttpOnly = true,
                IsEssential = true
            };

            Response.Cookies.Append(PreferenceCookieName, preferredPriority, options);

            TempData["Success"] = $"Your preferred request priority was saved as {preferredPriority}.";
            return RedirectToAction(nameof(Index));
        }

        // ---------------------------------------------------------------
        // GET: /SupplyRequest/MyRequests  -> all confirmed requests
        // ---------------------------------------------------------------
        [HttpGet]
        public IActionResult MyRequests()
        {
            List<SupplyRequest> requests = SupplyRequestStore.GetAll();
            return View(requests);
        }

        // ---------------------------------------------------------------
        // GET: /SupplyRequest/About
        // ---------------------------------------------------------------
        [HttpGet]
        public IActionResult About()
        {
            ViewBag.ApplicationName = "University Cafeteria Supply Management System";
            ViewBag.FeatureName = "Supply Request Management";
            ViewBag.Description = "This feature helps cafeteria employees create, manage, review, and calculate supply requests.";
            return View();
        }

        // ===============================================================
        // Calculations (all done inside the controller)
        // ===============================================================
        private SupplyRequest CalculateRequest(List<SupplyItem> items)
        {
            var request = new SupplyRequest { Items = items };

            // 1 + 2. Each product's total cost, then the subtotal of all products
            decimal subtotal = 0;
            foreach (var item in items)
            {
                subtotal += item.Quantity * item.UnitPrice;
            }
            request.Subtotal = subtotal;

            // The request priority is the highest priority selected among its products
            request.Priority = GetRequestPriority(items);

            // 3. Preparation fee based on the priority (switch statement)
            request.PreparationFee = GetPreparationFee(request.Priority);

            // 4. Discount based on the subtotal (conditional statements)
            decimal discountPercent;
            if (subtotal > 200)
            {
                discountPercent = 15;
            }
            else if (subtotal > 100)
            {
                discountPercent = 10;
            }
            else
            {
                discountPercent = 0;
            }
            request.DiscountPercent = discountPercent;
            request.Discount = Math.Round(subtotal * discountPercent / 100, 2);

            // 5. Final total
            request.FinalTotal = request.Subtotal + request.PreparationFee - request.Discount;

            return request;
        }

        private static decimal GetPreparationFee(string priority)
        {
            decimal fee;
            switch (priority)
            {
                case "Normal":
                    fee = 2;
                    break;
                case "Urgent":
                    fee = 5;
                    break;
                case "Emergency":
                    fee = 10;
                    break;
                default:
                    fee = 2;
                    break;
            }
            return fee;
        }

        private static int GetPriorityLevel(string priority)
        {
            switch (priority)
            {
                case "Emergency":
                    return 3;
                case "Urgent":
                    return 2;
                default:
                    return 1;
            }
        }

        private static string GetRequestPriority(List<SupplyItem> items)
        {
            string result = "Normal";
            foreach (var item in items)
            {
                if (GetPriorityLevel(item.Priority) > GetPriorityLevel(result))
                {
                    result = item.Priority;
                }
            }
            return result;
        }

        // ===============================================================
        // Session / Cookie helpers
        // ===============================================================
        private List<SupplyItem> GetDraft()
        {
            string? json = HttpContext.Session.GetString(DraftSessionKey);
            if (string.IsNullOrEmpty(json))
            {
                return new List<SupplyItem>();
            }
            return JsonSerializer.Deserialize<List<SupplyItem>>(json) ?? new List<SupplyItem>();
        }

        private void SaveDraft(List<SupplyItem> draft)
        {
            HttpContext.Session.SetString(DraftSessionKey, JsonSerializer.Serialize(draft));
        }

        private string GetPreferredPriority()
        {
            string? value = Request.Cookies[PreferenceCookieName];
            return value != null && Priorities.Contains(value) ? value : "Normal";
        }

        private void FillIndexViewBag(string preferred)
        {
            ViewBag.Priorities = Priorities;
            ViewBag.Categories = Categories;
            ViewBag.DraftItems = GetDraft();
            ViewBag.PreferredPriority = preferred;
            ViewBag.HasPreferenceCookie = Request.Cookies.ContainsKey(PreferenceCookieName);
        }
    }
}
