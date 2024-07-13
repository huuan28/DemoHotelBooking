using DemoHotelBooking.Models;
using DemoHotelBooking.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace DemoHotelBooking.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly AppDbContext _context;

        public AccountController(AppDbContext context,UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _context = context;
        }

        public async Task<IActionResult> CustomerList()
        {
            //var users = _context.Users.ToList();
            var users = _userManager.Users.ToList();
            var cus = new List<AppUser>();
            foreach(var u in users)
            {
                if (await _userManager.IsInRoleAsync(u, "Customer"))
                {
                    cus.Add(u);
                }
            }
            return View(cus);
        }
        public async Task<IActionResult> StaffList()
        {
            //var users = _context.Users.ToList();
            var users = _userManager.Users.ToList();
            var cus = new List<AppUser>();
            foreach (var u in users)
            {
                if (await _userManager.IsInRoleAsync(u, "Customer"))
                {
                    cus.Add(u);
                }
            }
            return View(cus);
        }

        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new AppUser
                {
                    UserName = model.Email,
                    Email = model.Email,
                    FullName = model.FullName,
                    IsRegisted = true,
                    PhoneNumber = model.PhoneNumber
                };
                var result = await _userManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    // Kiểm tra và tạo vai trò "Customer" nếu chưa có
                    if (!await _roleManager.RoleExistsAsync("Customer"))
                    {
                        var role = new IdentityRole("Customer");
                        await _roleManager.CreateAsync(role);
                    }

                    // Gán vai trò "Customer" cho người dùng
                    await _userManager.AddToRoleAsync(user, "Customer");

                    await _signInManager.SignInAsync(user, isPersistent: false);
                    return RedirectToAction("Index", "Home");
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            return View(model);
        }
        public IActionResult Login(string? returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model, string? returnUrl = null)
        {
            if (ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync(model.UserName, model.Password, model.RememberMe, lockoutOnFailure: false);
                if (result.Succeeded)
                {
                    if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                    {
                        return Redirect(returnUrl);
                    }
                    else
                    {
                        return RedirectToAction("Index", "Home");
                    }
                }
                ModelState.AddModelError(string.Empty, "Invalid login attempt.");
            }
            // Ensure the returnUrl is preserved in case of an error
            ViewData["ReturnUrl"] = returnUrl;
            return View(model);
        }

        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}
