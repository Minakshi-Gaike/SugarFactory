
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using VeariumTraders.Data;

namespace VeariumTraders.Controllers
{
    public class AdminController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly SignInManager<IdentityUser> _signInManager;

        public AdminController(
            ApplicationDbContext context,
            SignInManager<IdentityUser> signInManager)
        {
            _context = context;
            _signInManager = signInManager;
        }

        // Display Login Page
        [AllowAnonymous]
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        // Validate Admin Credentials using Identity
        [AllowAnonymous]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(
            string username,
            string password)
        {
            var result = await _signInManager.PasswordSignInAsync(
                username,
                password,
                isPersistent: false,
                lockoutOnFailure: true);

            if (result.Succeeded)
            {
                return RedirectToAction(nameof(Index));
            }

            if (result.IsLockedOut)
            {
                ViewBag.Error =
                    "Too many failed attempts. Please try again after 15 minutes.";
            }
            else
            {
                ViewBag.Error = "Invalid username or password.";
            }

            return View();
        }

        // Admin Dashboard - Only for Admin
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Index()
        {
            var enquiries = await _context.BulkEnquiries
                .OrderByDescending(e => e.EnquiryDate)
                .ToListAsync();

            return View(enquiries);
        }

        // Admin Logout
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();

            return RedirectToAction(nameof(Login));
        }
    }
}
