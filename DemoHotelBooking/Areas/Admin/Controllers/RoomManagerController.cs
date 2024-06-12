using DemoHotelBooking.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DemoHotelBooking.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class RoomManagerController : Controller
    {
        public AppDbContext _context { get; set; }
        public RoomManagerController(AppDbContext context)
        {
            _context = context;
        }        
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(Room room)
        {
            if (ModelState.IsValid)
            {
                Room newRoom = _context.Rooms.FirstOrDefault(i=> i.Id == room.Id);
                if (newRoom == null)
                {
                    _context.Rooms.Add(room);
                    await _context.SaveChangesAsync();
                    return RedirectToAction("AllRoomList", "RoomManager", new {area = "Admin"});
                }
            }
            ViewBag.Error = "Thông tin phòng không hợp lệ";
            return View(room);
        }
        public IActionResult AllRoomList()
        {
            var list = _context.Rooms.ToList();
            return View(list);
        }

    }
}
