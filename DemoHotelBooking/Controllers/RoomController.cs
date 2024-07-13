using DemoHotelBooking.Models;
using DemoHotelBooking.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DemoHotelBooking.Controllers
{
    public class RoomController : Controller
    {
        public readonly AppDbContext _context;
        private readonly UserManager<AppUser> _userManager;
        public RoomController(AppDbContext context, UserManager<AppUser> userManager)
        {
            _context = context;
            _userManager=userManager;
        }

        public IActionResult Index()
        {
            var list = _context.Feedbacks.Include(a=>a.User).ToList();
            return View(list);
        }
        public IActionResult Rooms(string? s)
        {
            var list = _context.Rooms.ToList();
            if (!string.IsNullOrEmpty(s))
            {
                s = s.ToLower();
                int id;
                if (int.TryParse(s, out id))
                    list = list.Where(i => i.Name.ToLower().Contains(s) || i.Type.ToLower().Contains(s)||i.Id==id).ToList();
                else
                    list = list.Where(i=>i.Name.ToLower().Contains(s)||i.Type.ToLower().Contains(s)).ToList();
            }
            return View(list);
        }
        [HttpPost]
        public IActionResult RoomList(string s)
        {
            var list = _context.Rooms.ToList();
            if (!string.IsNullOrEmpty(s))
            {
                s = s.ToLower();
                int id;
                if (int.TryParse(s, out id))
                    list = list.Where(i => i.Name.ToLower().Contains(s) || i.Type.ToLower().Contains(s) || i.Id == id).ToList();
                else
                    list = list.Where(i => i.Name.ToLower().Contains(s) || i.Type.ToLower().Contains(s)).ToList();
            }
            return PartialView("RoomList",list);
        }
        public IActionResult Details(int id)
        {
            var room = _context.Rooms.Find(id);
            if (room == null)
                return NotFound();
            return View(room);
        }
        [HttpPost]
        public async Task<IActionResult> Create(int stars, string comment)
        {
            if (ModelState.IsValid)
            {
                var customer = await _userManager.GetUserAsync(HttpContext.User);
                var feedbacks = new Feedback
                {
                    Stars = stars,
                    Comment = comment,
                    CusId = customer.Id,
                    CreateDate = DateTime.Now,
                    Status = true
                };
                _context.Feedbacks.Add(feedbacks);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View();
        }

    }
}
