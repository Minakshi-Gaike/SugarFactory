
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VeariumTraders.Data;

namespace VeariumTraders.Controllers
{
    public class AdminController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AdminController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var enquiries = await _context.BulkEnquiries
                .OrderByDescending(e => e.EnquiryDate)
                .ToListAsync();

            return View(enquiries);
        }
    }
}
