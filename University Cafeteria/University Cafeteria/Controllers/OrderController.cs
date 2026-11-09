using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using University_Cafeteria.Data;
using University_Cafeteria.Models;

namespace University_Cafeteria.Controllers
{
    public class OrderController : Controller
    {

        CafeteriaDbContext db;

        public OrderController(CafeteriaDbContext _db)
        {
            db = _db;
        }

        public IActionResult Index()
        {
            var orders = db.Orders
                           .Include(o => o.Employee)
                           .ToList();
            return View(orders);
        }

        [HttpGet]
        public IActionResult Create()
        {
            FillDropdowns();
            return View(new Order { OrderDate = DateTime.Now });
        }

        [HttpPost]
        public IActionResult Create(Order order)
        {
            db.Add(order);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public IActionResult Details(int id)
        {
            var order = db.Orders
                          .Include(o => o.Employee)
                          .FirstOrDefault(o => o.Id == id);

            if (order == null)
            {
                return NotFound();
            }

            return View(order);
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            var order = db.Orders.Find(id);

            if (order == null)
            {
                return NotFound();
            }

            FillDropdowns();
            return View(order);
        }

        [HttpPost]
        public IActionResult Edit(Order order)
        {
            db.Update(order);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult Delete(int id)
        {
            var order = db.Orders.Find(id);

            if (order == null)
            {
                return NotFound();
            }

            db.Remove(order);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        private void FillDropdowns()
        {
            ViewBag.Employees = new SelectList(db.Employees.ToList(), "Id", "FullName");
        }

    }
}
