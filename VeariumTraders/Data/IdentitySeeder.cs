
using Microsoft.AspNetCore.Identity;

namespace VeariumTraders.Data
{
    public static class IdentitySeeder
    {
        public static async Task SeedAdminAsync(
            IServiceProvider serviceProvider,
            IConfiguration configuration)
        {
            using var scope = serviceProvider.CreateScope();

            var userManager = scope.ServiceProvider
                .GetRequiredService<UserManager<IdentityUser>>();

            var roleManager = scope.ServiceProvider
                .GetRequiredService<RoleManager<IdentityRole>>();

            var username = configuration["AdminLogin:Username"];
            var password = configuration["AdminLogin:Password"];

            if (string.IsNullOrWhiteSpace(username) ||
                string.IsNullOrWhiteSpace(password))
            {
                throw new InvalidOperationException(
                    "AdminLogin username or password is missing.");
            }

            // Create Admin role if it does not exist
            if (!await roleManager.RoleExistsAsync("Admin"))
            {
                var roleResult = await roleManager.CreateAsync(
                    new IdentityRole("Admin"));

                if (!roleResult.Succeeded)
                {
                    throw new InvalidOperationException(
                        string.Join("; ",
                            roleResult.Errors.Select(e => e.Description)));
                }
            }

            // Find existing admin user
            var admin = await userManager.FindByNameAsync(username);

            // Create user only if it does not exist
            if (admin == null)
            {
                admin = new IdentityUser
                {
                    UserName = username,
                    Email = "gaykeminakshi139@gmail.com",
                    EmailConfirmed = false
                };

                var createResult = await userManager.CreateAsync(
                    admin, password);

                if (!createResult.Succeeded)
                {
                    throw new InvalidOperationException(
                        string.Join("; ",
                            createResult.Errors.Select(e => e.Description)));
                }
            }

            // Confirm the configured Admin email
            if (admin.Email == "gaykeminakshi139@gmail.com" &&
                !admin.EmailConfirmed)
            {
                admin.EmailConfirmed = true;

                var updateResult = await userManager.UpdateAsync(admin);

                if (!updateResult.Succeeded)
                {
                    throw new InvalidOperationException(
                        string.Join("; ",
                            updateResult.Errors.Select(e => e.Description)));
                }
            }


            // Assign Admin role
            if (!await userManager.IsInRoleAsync(admin, "Admin"))
            {
                var roleResult = await userManager.AddToRoleAsync(
                    admin, "Admin");

                if (!roleResult.Succeeded)
                {
                    throw new InvalidOperationException(
                        string.Join("; ",
                            roleResult.Errors.Select(e => e.Description)));
                }
            }
        }
    }
}
