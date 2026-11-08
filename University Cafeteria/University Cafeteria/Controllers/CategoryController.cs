using Microsoft.AspNetCore.Mvc;
using University_Cafeteria.Data;
using University_Cafeteria.Models;

namespace University_Cafeteria.Controllers
{
    public class CategoryController : Controller
    {

        CafeteriaDbContext db;

       public CategoryController(CafeteriaDbContext _db)
        {
            db = _db;
        }

        public IActionResult Index()
        {
            var categories =  db.Categories.ToList();
            return View(categories);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Category category)
        {
            db.Add(category);
            db.SaveChanges();
            return RedirectToAction("index");
        }



    }
}
