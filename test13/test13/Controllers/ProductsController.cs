using Microsoft.AspNetCore.Mvc;
using test13.Data;
using test13.Models;

namespace test13.Controllers
{
    public class ProductsController : Controller
    {

        private readonly AppDbContext db;

        public ProductsController(AppDbContext context)
        {
            db = context;
        }

        public ActionResult Index()
        {
            var products = db.Products.ToList();   
            return View(products);                
        }


        [HttpGet]
        public ActionResult Create()
        {
            return View();   
        }

        [HttpPost]
        public IActionResult Create(Product product)
        {
            if(ModelState.IsValid)
            {
                db.Products.Add(product);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(product);
        }


    }
}
