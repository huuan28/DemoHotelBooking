using Microsoft.AspNetCore.Identity;

namespace DemoHotelBooking.Models
{
    public static class SeedData
    {
        public static async Task Initialize(UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            // Tạo vai trò mặc định nếu chưa tồn tại
            if (!await roleManager.RoleExistsAsync("Admin"))
            {
                await roleManager.CreateAsync(new IdentityRole("Admin"));
            }

            if (!await roleManager.RoleExistsAsync("Customer"))
            {
                await roleManager.CreateAsync(new IdentityRole("Customer"));
            }
            if (!await roleManager.RoleExistsAsync("Receptionist"))
            {
                await roleManager.CreateAsync(new IdentityRole("Receptionist"));
            }
            if (!await roleManager.RoleExistsAsync("Accountant"))
            {
                await roleManager.CreateAsync(new IdentityRole("Accountant"));
            }
            // Tạo tài khoản admin mặc định nếu chưa tồn tại
            if (userManager.Users.All(u => u.UserName != "admin"))
            {
                var admin = new AppUser { UserName = "admin", Email = "admin@example.com" };
                var result = await userManager.CreateAsync(admin, "admin");

                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(admin, "Admin");
                }
            }

            // Danh sách các khách hàng cần thêm
            var customers = new List<AppUser>
            {
                 new AppUser { UserName = "an",FullName = "An", Email = "customer1@example.com" },
                 new AppUser { UserName = "hao",FullName = "Hào", Email = "customer2@example.com" },
                 new AppUser { UserName = "manh",FullName = "Mạnh", Email = "customer3@example.com" },
                 new AppUser { UserName = "tuan",FullName = "Tuấn", Email = "customer5@example.com" },
             };
            if (userManager.Users.All(u => u.UserName != "letan"))
            {
                var user = new AppUser { UserName = "letan" };
                var result = await userManager.CreateAsync(user, "123123");
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(user, "Receptionist");
                }
            }
            if (userManager.Users.All(u => u.UserName != "ketoan"))
            {
                var user = new AppUser { UserName = "ketoan" };
                var result = await userManager.CreateAsync(user, "123123");
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(user, "Accountant");
                }
            }
            var password = "123123";

            foreach (var customer in customers)
            {
                if (userManager.Users.All(u => u.UserName != customer.UserName))
                {
                    var result = await userManager.CreateAsync(customer, password);
                    if (result.Succeeded)
                    {
                        await userManager.AddToRoleAsync(customer, "Customer");
                    }
                }
            }
        }
    }
}
