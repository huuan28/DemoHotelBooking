using DemoHotelBooking.Models;
using DemoHotelBooking.ViewModels;
using Microsoft.AspNetCore.Authorization;
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
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            var list = _context.Feedbacks.Where(i=>i.Status==true).Include(a => a.User).ToList();
            var customer = await _userManager.GetUserAsync(HttpContext.User);
            if (customer != null)
            {
                ViewData["user"]=customer;
                var fb = list.FirstOrDefault(i => i.CusId == customer.Id);
                if (fb != null)
                    ViewData["feedback"] = fb;
                if (_context.Bookings.Any(i => i.CusID == customer.Id))
                    ViewBag.flag = true;
                else ViewBag.flag = false;
            }
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
                    list = list.Where(i => i.Name.ToLower().Contains(s) || i.Type.ToLower().Contains(s) || i.Id == id).ToList();
                else
                    list = list.Where(i => i.Name.ToLower().Contains(s) || i.Type.ToLower().Contains(s)).ToList();
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
            return PartialView("RoomList", list);
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
                var feedbacks = _context.Feedbacks.FirstOrDefault(i => i.CusId == customer.Id);
                if (feedbacks == null)
                {
                    feedbacks = new Feedback
                    {
                        Stars = stars,
                        Comment = comment,
                        CusId = customer.Id,
                        CreateDate = DateTime.Now,
                        Status = true
                    };
                    _context.Feedbacks.Add(feedbacks);
                }
                feedbacks.Comment = comment;
                feedbacks.Stars = stars;
                feedbacks.EditDate = DateTime.Now;
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View();
        }
        [Authorize(Roles ="Admin")]
        public IActionResult HideFeedBack(string id)
        {
            var fb = _context.Feedbacks.Find(id);
            if(fb != null) fb.Status= false;
            _context.Feedbacks.Update(fb);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}
