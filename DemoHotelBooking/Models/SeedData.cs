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
                 new AppUser { UserName = "customer1@example.com",FullName = "An", Email = "customer1@example.com" },
                 new AppUser { UserName = "customer2@example.com",FullName = "Hao", Email = "customer2@example.com" },
                 new AppUser { UserName = "customer3@example.com",FullName = "Manh", Email = "customer3@example.com" },
                 new AppUser { UserName = "customer4@example.com",FullName = "Hien", Email = "customer4@example.com" },
                 new AppUser { UserName = "customer5@example.com",FullName = "Tuan", Email = "customer5@example.com" },
                 new AppUser { UserName = "khachhang",FullName = "Khách hàng", Email = "customer5@example.com" },
             };

            var password = "khachhang123";

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
