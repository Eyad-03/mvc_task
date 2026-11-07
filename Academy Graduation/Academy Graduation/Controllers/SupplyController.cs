using Microsoft.AspNetCore.Mvc;
using Academy_Graduation.Models;
using System.Text.Json;
namespace Academy_Graduation.Controllers
{
    public class SupplyController : Controller
    {
        [HttpGet]
        public IActionResult Create()
        {
            return View(SupplyData.Categories);
        }

        [HttpPost]
        public IActionResult Create(int cat1,int cat2,int cat3)
        {
            if(cat1==0 || cat2==0 ||cat3==0)
            {
                TempData["Error"] = "Please select one offer from each category.";
                RedirectToAction("Create");

            }


            HttpContext.Session.SetInt32("CertificateOffer", cat1);
            HttpContext.Session.SetInt32("HospitalityOffer", cat2);
            HttpContext.Session.SetInt32("GiftOffer", cat3);

            var selected = new List<int> { cat1, cat2, cat3 };
            HttpContext.Session.SetString("SelectedOffers", JsonSerializer.Serialize(selected));

            return RedirectToAction("Review");

        }


        public IActionResult Review()
        {
            var json = HttpContext.Session.GetString("SelectedOffers");

            if(json==null)
            {
                TempData["Error"] = "Please select your offers first.";
                return RedirectToAction("Create");
            }


            var ids = JsonSerializer.Deserialize<List<int>>(json);

            if (ids.Count != 3 || ids==null)
            {
                TempData["Error"] = "You must select 3 offers.";
                return RedirectToAction("Create");
            }


            var items = new List<ReviewItem>();
            foreach(var cat in SupplyData.Categories)
            {

                foreach(var sup in cat.Suppliers)
                {
                    if(ids.Contains(sup.Offer.Id))
                    {

                        items.Add(new ReviewItem
                        {
                            Category = cat.Name,
                            Supplier = sup.Name,
                            Offer = sup.Offer.Name,
                            Quantity = sup.Offer.Quantity,
                            UnitPrice = sup.Offer.UnitPrice,
                            Total = sup.Offer.Quantity * sup.Offer.UnitPrice

                        });


                    }
                }


            }

            decimal totalPrice = items.Sum(i => i.Total);
            decimal discountPercent;
            if (totalPrice > 500)
                discountPercent = 15;
            else if (totalPrice > 300)
                discountPercent = 10;
            else
                discountPercent = 0;

            decimal discountAmount = totalPrice * discountPercent / 100;
            decimal finalTotal = totalPrice - discountAmount;

            ViewBag.TotalPrice = totalPrice;
            ViewBag.DiscountPercent = discountPercent;
            ViewBag.DiscountAmount = discountAmount;
            ViewBag.FinalTotal = finalTotal;

            return View(items);


        }



        public IActionResult About()
        {
            return View();
        }


    }
}
