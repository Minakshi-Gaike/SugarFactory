using Microsoft.AspNetCore.Mvc;

namespace VeariumTraders.Controllers
{
    public class ProductsController : Controller
    {
        public IActionResult Index()
        {
            ViewData["Title"] = "Products";
            return View();
        }
    }
}
