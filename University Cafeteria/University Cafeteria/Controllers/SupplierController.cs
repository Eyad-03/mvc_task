using Microsoft.AspNetCore.Mvc;
using University_Cafeteria.Data;
using University_Cafeteria.Models;

namespace University_Cafeteria.Controllers
{
    public class SupplierController : Controller
    {

        CafeteriaDbContext db;

        public SupplierController(CafeteriaDbContext _db)
        {
            db = _db;
        }

        public IActionResult Index()
        {
            var suppliers = db.Suppliers.ToList();
            return View(suppliers);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Supplier supplier)
        {
            db.Add(supplier);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

    }
}