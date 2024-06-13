using DemoHotelBooking.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DemoHotelBooking.Areas.Receptionist.Controllers
{
    [Area("Receptionist")]
    [Authorize(Roles = "Receptionist")]
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
    }
}
