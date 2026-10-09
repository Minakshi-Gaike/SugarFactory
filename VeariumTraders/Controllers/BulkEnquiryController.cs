using Microsoft.AspNetCore.Mvc;
using VeariumTraders.Data;
using VeariumTraders.Models;

namespace VeariumTraders.Controllers
{
    public class BulkEnquiryController : Controller
    {
        private readonly ApplicationDbContext _context;
        public BulkEnquiryController(ApplicationDbContext context)
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
        public async Task<IActionResult> Index(BulkEnquiry enquiry)
        {
            if (!ModelState.IsValid)
            {
                return View(enquiry);
            }

            enquiry.EnquiryDate = DateTime.Now;
            enquiry.Status = "Pending";
            try
            {
                _context.BulkEnquiries.Add(enquiry);
                await _context.SaveChangesAsync();
            }
            catch (Microsoft.EntityFrameworkCore.DbUpdateException ex)
            {
                System.Diagnostics.Debug.WriteLine(
                    "Database Error: " + ex.InnerException?.Message);

                throw;
            }

            TempData["SuccessMessage"] =
                "Your bulk enquiry has been submitted successfully.";

            return RedirectToAction(nameof(Index));
        }
    }
}
