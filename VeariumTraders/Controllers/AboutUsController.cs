using Microsoft.AspNetCore.Mvc;

namespace VeariumTraders.Controllers
{
    public class AboutUsController : Controller
    {
        public IActionResult Index()
        {
            ViewData["Title"] = "About Us";
            return View();
        }
    }
}
