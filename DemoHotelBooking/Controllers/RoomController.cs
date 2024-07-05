using DemoHotelBooking.Models;
using Microsoft.AspNetCore.Mvc;

namespace DemoHotelBooking.Controllers
{
    public class RoomController : Controller
    {
        public readonly AppDbContext _context;
        public RoomController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var list = _context.Rooms.ToList();
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

    }
}
