
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VeariumTraders.Data;
using VeariumTraders.Models;

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


        // Display Change Password Page
        [Authorize(Roles = "Admin")]
        [HttpGet]
        public IActionResult ChangePassword()
        {
            return View(new AdminChangePasswordViewModel());
        }

        // Change Admin Password using Identity
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangePassword(
            AdminChangePasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = await _signInManager.UserManager
                .GetUserAsync(User);

            if (user == null)
            {
                return RedirectToAction(nameof(Login));
            }

            var result = await _signInManager.UserManager
                .ChangePasswordAsync(
                    user,
                    model.CurrentPassword,
                    model.NewPassword);

            if (result.Succeeded)
            {
                await _signInManager.RefreshSignInAsync(user);

                TempData["Success"] =
                    "Password changed successfully.";

                return RedirectToAction(nameof(ChangePassword));
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(
                    string.Empty,
                    error.Description);
            }

            return View(model);
        }

    


// Display Forgot Password Page
[AllowAnonymous]
        [HttpGet]
        public IActionResult ForgotPassword()
        {
            return View();
        }

        // Send Password Reset Link
        [AllowAnonymous]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ForgotPassword(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
            {
                ViewBag.Message =
                    "If the email is registered, a reset link will be sent.";
                return View();
            }

            var user = await _signInManager.UserManager
                .FindByEmailAsync(email);

            // Generic message prevents revealing whether the account exists
            if (user != null &&
                await _signInManager.UserManager
                    .IsEmailConfirmedAsync(user))
            {
                var token = await _signInManager.UserManager
                    .GeneratePasswordResetTokenAsync(user);

                var encodedToken = Microsoft.AspNetCore.WebUtilities
                    .WebEncoders.Base64UrlEncode(
                        System.Text.Encoding.UTF8.GetBytes(token));

                var resetLink = Url.Action(
                    nameof(ResetPassword),
                    "Admin",
                    new { email = user.Email, token = encodedToken },
                    protocol: Request.Scheme);

                try
                {
                    var emailSender = HttpContext.RequestServices
                        .GetRequiredService<
                            VeariumTraders.Services.EmailSender>();

                    await emailSender.SendEmailAsync(
                        user.Email!,
                        "Vearium Traders - Reset Password",
                        $"""
                        <h2>Reset Your Password</h2>
                        <p>We received a request to reset your password.</p>
                        <p><a href="{System.Net.WebUtility.HtmlEncode(resetLink)}">Reset Password</a></p>
                        <p>If you did not request this, you can ignore this email.</p>
                        """);
                }
                catch (Exception)
                {
                    // Do not reveal account details or SMTP errors to visitors.
                    // Configure application logging for production diagnostics.
                }
            }

            ViewBag.Message =
                "If the email is registered and confirmed, a reset link will be sent.";

            return View();
        }

        // Display Reset Password Page
        [AllowAnonymous]
        [HttpGet]
        public IActionResult ResetPassword(string email, string token)
        {
            if (string.IsNullOrWhiteSpace(email) ||
                string.IsNullOrWhiteSpace(token))
            {
                return BadRequest("Invalid password reset link.");
            }

            ViewBag.Email = email;
            ViewBag.Token = token;

            return View();
        }

        // Reset Password using Identity token
        [AllowAnonymous]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ResetPassword(
            string email,
            string token,
            string newPassword,
            string confirmPassword)
        {
            if (string.IsNullOrWhiteSpace(email) ||
                string.IsNullOrWhiteSpace(token) ||
                string.IsNullOrWhiteSpace(newPassword) ||
                string.IsNullOrWhiteSpace(confirmPassword))
            {
                ViewBag.Error = "Please fill in all fields.";
                ViewBag.Email = email;
                ViewBag.Token = token;
                return View();
            }

            if (newPassword != confirmPassword)
            {
                ViewBag.Error = "Passwords do not match.";
                ViewBag.Email = email;
                ViewBag.Token = token;
                return View();
            }

            var decodedToken = System.Text.Encoding.UTF8.GetString(
                Microsoft.AspNetCore.WebUtilities.WebEncoders
                    .Base64UrlDecode(token));

            var user = await _signInManager.UserManager
                .FindByEmailAsync(email);

            if (user == null)
            {
                ViewBag.Error = "The reset link is invalid or expired.";
                ViewBag.Email = email;
                ViewBag.Token = token;
                return View();
            }

            var result = await _signInManager.UserManager
                .ResetPasswordAsync(user, decodedToken, newPassword);

            if (result.Succeeded)
            {
                TempData["ResetSuccess"] =
                    "Password reset successful. Please log in.";

                return RedirectToAction(nameof(Login));
            }

            ViewBag.Error = string.Join(
                " ",
                result.Errors.Select(e => e.Description));

            ViewBag.Email = email;
            ViewBag.Token = token;

            return View();
        }

    }
}