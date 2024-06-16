using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace DemoHotelBooking.Models
{
    public static class SeedData
    {
        public static async Task Initialize(UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager, IApplicationBuilder app)
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

            AppDbContext context = app.ApplicationServices.
                CreateScope().ServiceProvider.GetRequiredService<AppDbContext>();
            if (context.Database.GetPendingMigrations().Any())
            {
                context.Database.Migrate();
            }
            if (!context.Rooms.Any())
            {
                context.AddRange(
                                 new Room
                                 {
                                     Name = "STD101",
                                     Type = "Standard",
                                     FloorNumber = 1,
                                     Price = 50.00,
                                     Introduce = "Phòng tiêu chuẩn với giường đơn.",
                                     Description = "Phòng tiêu chuẩn với một giường đơn, phòng tắm riêng và view núi.",
                                     Visio = "View núi",
                                     DAP = 1,
                                     MAP = 2
                                 },
                new Room
                {
                    Name = "STD102",
                    Type = "Standard",
                    FloorNumber = 1,
                    Price = 55.00,
                    Introduce = "Phòng tiêu chuẩn với giường đôi.",
                    Description = "Phòng tiêu chuẩn với một giường đôi, phòng tắm riêng và view biển.",
                    Visio = "View biển",
                    DAP = 2,
                    MAP = 2
                },
                new Room
                {
                    Name = "SUP201",
                    Type = "Superior",
                    FloorNumber = 2,
                    Price = 70.00,
                    Introduce = "Phòng superior với giường đôi và tiện nghi hiện đại.",
                    Description = "Phòng superior với một giường đôi lớn, phòng tắm rộng rãi và view biển.",
                    Visio = "View biển",
                    DAP = 2,
                    MAP = 3
                },
                new Room
                {
                    Name = "SUP202",
                    Type = "Superior",
                    FloorNumber = 2,
                    Price = 75.00,
                    Introduce = "Phòng superior với giường đôi và view núi.",
                    Description = "Phòng superior với một giường đôi lớn, phòng tắm riêng và view núi.",
                    Visio = "View núi",
                    DAP = 2,
                    MAP = 3
                },
                new Room
                {
                    Name = "DLX301",
                    Type = "Deluxe",
                    FloorNumber = 3,
                    Price = 90.00,
                    Introduce = "Phòng deluxe với giường đôi và view biển.",
                    Description = "Phòng deluxe với một giường đôi lớn, phòng tắm riêng và ban công rộng view biển.",
                    Visio = "View biển",
                    DAP = 2,
                    MAP = 4
                },
                new Room
                {
                    Name = "DLX302",
                    Type = "Deluxe",
                    FloorNumber = 3,
                    Price = 95.00,
                    Introduce = "Phòng deluxe với giường đôi và view núi.",
                    Description = "Phòng deluxe với một giường đôi lớn, phòng tắm riêng và ban công rộng view núi.",
                    Visio = "View núi",
                    DAP = 2,
                    MAP = 4
                },
                new Room
                {
                    Name = "SUT401",
                    Type = "Suite",
                    FloorNumber = 4,
                    Price = 120.00,
                    Introduce = "Phòng suite với phòng khách riêng và view biển.",
                    Description = "Phòng suite với một giường đôi lớn, phòng tắm riêng, phòng khách và ban công view biển.",
                    Visio = "View biển",
                    DAP = 2,
                    MAP = 5
                },
                new Room
                {
                    Name = "SUT402",
                    Type = "Suite",
                    FloorNumber = 4,
                    Price = 125.00,
                    Introduce = "Phòng suite với phòng khách riêng và view núi.",
                    Description = "Phòng suite với một giường đôi lớn, phòng tắm riêng, phòng khách và ban công view núi.",
                    Visio = "View núi",
                    DAP = 2,
                    MAP = 5
                },
                new Room
                {
                    Name = "FAM501",
                    Type = "Family",
                    FloorNumber = 5,
                    Price = 140.00,
                    Introduce = "Phòng family với hai giường đôi.",
                    Description = "Phòng family với hai giường đôi, phòng tắm riêng và ban công rộng view biển.",
                    Visio = "View biển",
                    DAP = 4,
                    MAP = 6
                },
                new Room
                {
                    Name = "FAM502",
                    Type = "Family",
                    FloorNumber = 5,
                    Price = 145.00,
                    ImagePath = "/images/room10.jpg",
                    Introduce = "Phòng family với hai giường đôi và view núi.",
                    Description = "Phòng family với hai giường đôi, phòng tắm riêng và ban công rộng view núi.",
                    Visio = "View núi",
                    DAP = 4,
                    MAP = 6
                }
            );
                context.SaveChanges();
            }
        }
    }
}
