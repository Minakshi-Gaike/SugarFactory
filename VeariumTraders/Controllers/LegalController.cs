
using Microsoft.AspNetCore.Mvc;

namespace VeariumTraders.Controllers
{
    public class LegalController : Controller
    {
        public IActionResult TermsOfTrade()
        {
            return View();
        }

        public IActionResult PrivacyPolicy()
        {
            return View();
        }

        public IActionResult ShippingPolicy()
        {
            return View();
        }

        public IActionResult QualityGuarantee()
        {
            return View();
        }
    }
}
