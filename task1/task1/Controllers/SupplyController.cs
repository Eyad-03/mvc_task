using Microsoft.AspNetCore.Mvc;

namespace task1.Controllers
{
    public class SupplyController : Controller
    {
        public IActionResult Index()
        {

            string[] category = new string[] { "Drinks", "Food", "Snacks", "Hot Drinks", "Cold Drinks" };
            ViewBag.Category = category;

            return View();
        }


        public IActionResult Products()
        {

            ViewData["Title"] = "Cafeteria Products";
            ViewData["ManagerName"] = "Ahmad Ali";
            ViewData["count"] = 5;

            string [] arr = new string[] { "Water", "Orange Juice", "Coffee", "Sandwich", "Chocolate" };
        
            ViewData["Products"] = arr;

            return View();
        }


        public IActionResult Suppliers()
        {

            ViewBag.Title = "Our Suppliers";
            ViewBag.count = 3;

            string[] food = new string[] { "Fresh Food Company", "Happy Drinks Company", "Daily Supplies Company" }; ViewBag.Suppliers = food;

            return View();
        }


        [HttpGet]
        public IActionResult CreateRequest()
        {
            return View();
        }

        [HttpPost]
        public IActionResult SubmitRequest()
        {
            string employeeName = Request.Form["EmployeeName"];
            string productName = Request.Form["ProductName"];
            string quantity = Request.Form["Quantity"];
            string requiredDate = Request.Form["RequiredDate"];

            if (string.IsNullOrWhiteSpace(employeeName))
            {
                ViewBag.IsSuccess = false;
                ViewBag.Message = "Error: Employee Name is required. Please go back and fill it in.";
                return View();
            }

            TempData["Message"] = $"Success: Request for {quantity} x {productName} " +
                                  $"submitted by {employeeName} (required by {requiredDate}).";

            return RedirectToAction("About");
        }


        public IActionResult About()
        {
            return View();
        }



    }
}
