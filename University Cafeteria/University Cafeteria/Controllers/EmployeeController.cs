using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using University_Cafeteria.Data;
using University_Cafeteria.Models;

namespace University_Cafeteria.Controllers
{
    public class EmployeeController : Controller
    {

        CafeteriaDbContext db;

        public EmployeeController(CafeteriaDbContext _db)
        {
            db = _db;
        }

        public IActionResult Index()
        {
            var employees = db.Employees.Include(e => e.Orders).ToList();
            return View(employees);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Employee employee)
        {
            db.Add(employee);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public IActionResult Details(int id)
        {
            var employee = db.Employees
                             .Include(e => e.Orders)
                             .FirstOrDefault(e => e.Id == id);

            if (employee == null)
            {
                return NotFound();
            }

            return View(employee);
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            var employee = db.Employees.Find(id);

            if (employee == null)
            {
                return NotFound();
            }

            return View(employee);
        }

        [HttpPost]
        public IActionResult Edit(Employee employee)
        {
            db.Update(employee);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult Delete(int id)
        {
            var employee = db.Employees.Find(id);

            if (employee == null)
            {
                return NotFound();
            }

            db.Remove(employee);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

    }
}
