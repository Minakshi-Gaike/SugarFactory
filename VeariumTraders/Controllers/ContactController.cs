
using Microsoft.AspNetCore.Mvc;
using VeariumTraders.Data;
using VeariumTraders.Models;

namespace VeariumTraders.Controllers
{
    public class ContactController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ContactController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(
            string fullName,
            string email,
            string mobile,
            string? companyName,
            string serviceInquiry,
            string message)
        {
            if (string.IsNullOrWhiteSpace(fullName) ||
                string.IsNullOrWhiteSpace(email) ||
                string.IsNullOrWhiteSpace(mobile) ||
                string.IsNullOrWhiteSpace(serviceInquiry) ||
                string.IsNullOrWhiteSpace(message))
            {
                ViewBag.Error =
                    "Please fill in all required fields.";

                return View();
            }

            var enquiry = new ContactEnquiry
            {
                FullName = fullName,
                Email = email,
                Mobile = mobile,
                CompanyName = companyName,
                ServiceInquiry = serviceInquiry,
                Message = message,
                EnquiryDate = DateTime.Now,
                Status = "Pending"
            };

            _context.ContactEnquiries.Add(enquiry);

            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] =
                "Thank you! Your message has been received.";

            return RedirectToAction(nameof(Index));
        }
    }
}
