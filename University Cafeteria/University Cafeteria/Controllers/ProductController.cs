using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using University_Cafeteria.Data;
using University_Cafeteria.Models;

namespace University_Cafeteria.Controllers
{
    public class ProductController : Controller
    {

        CafeteriaDbContext db;

        public ProductController(CafeteriaDbContext _db)
        {
            db = _db;
        }

        public IActionResult Index()
        {
            var products = db.Products
                             .Include(p => p.Category)
                             .Include(p => p.Supplier)
                             .ToList();
            return View(products);
        }

        [HttpGet]
        public IActionResult Create()
        {
            FillDropdowns();
            return View();
        }

        [HttpPost]
        public IActionResult Create(Product product)
        {
            db.Add(product);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        private void FillDropdowns()
        {
            ViewBag.Categories = new SelectList(db.Categories.ToList(), "Id", "Name");
            ViewBag.Suppliers = new SelectList(db.Suppliers.ToList(), "Id", "Name");
        }

    }
}