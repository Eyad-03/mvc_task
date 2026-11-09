using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
            var suppliers = db.Suppliers.Include(s => s.Products).ToList();
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

        public IActionResult Details(int id)
        {
            var supplier = db.Suppliers.Find(id);

            if (supplier == null)
            {
                return NotFound();
            }

            return View(supplier);
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            var supplier = db.Suppliers.Find(id);

            if (supplier == null)
            {
                return NotFound();
            }

            return View(supplier);
        }

        [HttpPost]
        public IActionResult Edit(Supplier supplier)
        {
            db.Update(supplier);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult Delete(int id)
        {
            var supplier = db.Suppliers.Find(id);

            if (supplier == null)
            {
                return NotFound();
            }

            db.Remove(supplier);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

    }
}
