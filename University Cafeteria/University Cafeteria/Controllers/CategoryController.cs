using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
            var categories = db.Categories.Include(c => c.Products).ToList();
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


        public IActionResult Details(int id)
        {

            var categories = db.Categories.Find(id);

            if(categories == null)
            {
                return NotFound();
            }

            return View(categories);
        }


        [HttpGet]
        public IActionResult Edit(int id)
        {

            var category = db.Categories.Find(id);
            if(category == null)
            {
                return NotFound();
            }
            return View(category);
        }



        [HttpPost]
        public IActionResult Edit(Category category)
        {
            db.Update(category);
            db.SaveChanges();
            return RedirectToAction("index");
        }


        [HttpGet]
        public IActionResult Delete(int id)
        {
            var category = db.Categories.Find(id);
            if (category == null)
            {
                return NotFound();
            }
            db.Remove(category);
            db.SaveChanges();
            return RedirectToAction("index");
        }



    }
}
