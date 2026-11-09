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

        public IActionResult Details(int id)
        {
            var product = db.Products
                            .Include(p => p.Category)
                            .Include(p => p.Supplier)
                            .FirstOrDefault(p => p.Id == id);

            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            var product = db.Products.Find(id);

            if (product == null)
            {
                return NotFound();
            }

            FillDropdowns();
            return View(product);
        }

        [HttpPost]
        public IActionResult Edit(Product product)
        {
            db.Update(product);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult Delete(int id)
        {
            var product = db.Products.Find(id);

            if (product == null)
            {
                return NotFound();
            }

            db.Remove(product);
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
