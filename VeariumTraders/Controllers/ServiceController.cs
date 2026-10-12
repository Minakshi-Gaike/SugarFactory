using Microsoft.AspNetCore.Mvc;

namespace VeariumTraders.Controllers
{
    public class ServiceController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
