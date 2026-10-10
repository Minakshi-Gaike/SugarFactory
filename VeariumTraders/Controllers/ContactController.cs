using Microsoft.AspNetCore.Mvc;

namespace VeariumTraders.Controllers
{
    public class ContactController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
   
            [HttpPost]
            [ValidateAntiForgeryToken]
            public IActionResult Index(
                string fullName,
                string email,
                string mobile,
                string companyName,
                string serviceInquiry,
                string message)
            {
                if (string.IsNullOrWhiteSpace(fullName) ||
                    string.IsNullOrWhiteSpace(email) ||
                    string.IsNullOrWhiteSpace(mobile) ||
                    string.IsNullOrWhiteSpace(message))
                {
                    ViewBag.Error =
                        "Please fill in all required fields.";

                    return View();
                }

                // Next step: Save contact enquiry to database.

                ViewBag.Success =
                    "Thank you! Your message has been received.";

                return View();
            }
        }
    }
    

