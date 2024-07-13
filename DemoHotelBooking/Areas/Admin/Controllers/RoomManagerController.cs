using DemoHotelBooking.Models;
using DemoHotelBooking.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DemoHotelBooking.Areas.Admin.Controllers
{
    [Area("Admin")]
    //[Authorize(Roles = "Admin")]
    public class RoomManagerController : Controller
    {
        public AppDbContext _context { get; set; }
        public RoomManagerController(AppDbContext context)
        {
            _context = context;
        }
        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            return View(new RoomViewModel());
        }
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create(RoomViewModel model, IFormFile image)
        {
            if (ModelState.IsValid)
            {
                Room room = _context.Rooms.FirstOrDefault(i => i.Id == model.Id);
                if (room == null)
                {
                    room = new Room();
                    room.Name = model.Name;
                    room.Type = model.Type;
                    room.FloorNumber = model.FloorNumber;
                    room.Introduce = model.Introduce;
                    room.Description = model.Description;
                    room.MAP = model.MAP;
                    room.DAP = model.DAP;
                    room.Price = model.Price;
                    _context.Rooms.Add(room);
                    await _context.SaveChangesAsync();
                    return RedirectToAction("AllRoomList", "RoomManager", new { area = "Admin" });
                }
            }
            ViewBag.Error = "Thông tin phòng không hợp lệ";
            return View(model);
        }
        public IActionResult AllRoomList()
        {
            var list = _context.Rooms.ToList();
            return View(list);
        }
        [Authorize(Roles = "Admin")]
        public IActionResult Update(int Id)
        {
            var room = _context.Rooms.FirstOrDefault(r => r.Id == Id);
            if (room == null)
                return NotFound();
            return View(room);
        }
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Update(RoomViewModel model)
        {
            if (ModelState.IsValid)
            {
                var room = _context.Rooms.FirstOrDefault(i => i.Id == model.Id);
                if (room != null)
                {
                    room.Name = model.Name;
                    room.Type = model.Type;
                    room.FloorNumber = model.FloorNumber;
                    room.Introduce = model.Introduce;
                    room.Description = model.Description;
                    room.MAP = model.MAP;
                    room.DAP = model.DAP;
                    room.Price = model.Price;
                    _context.Rooms.Update(room);
                    await _context.SaveChangesAsync();
                    return RedirectToAction("AllRoomList", "RoomManager", new { area = "Admin" });
                }
            }
            ViewBag.Error = "Thông tin không hợp lệ";
            return View(model);
        }
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            var room = _context.Rooms.FirstOrDefault(i => i.Id == id);
            if (room != null)
            {
                _context.Rooms.Remove(room);
                await _context.SaveChangesAsync();
                return RedirectToAction("AllRoomList", "RoomManager", new { area = "Admin" });
            }
            return NotFound();
        }
        public IActionResult RoomStatus(DateTime? begin, DateTime? end)
        {
            DateTime b = begin?? DateTime.Now;
            DateTime e = end?? DateTime.Now;
            var list = _context.Rooms.ToList();
            var bookings = _context.Bookings.Where(i =>
                    (i.CheckinDate <= b && i.CheckoutDate >= e) ||
                    (i.CheckinDate <= b && i.CheckoutDate >= e) ||
                    (i.CheckinDate >= b && i.CheckoutDate <= e)).ToList();
            var bkdt = _context.BookingDetails.ToList();
            var bkdts = new List<BookingDetail>();
            foreach (var booking in bookings)
            {
                var dt = bkdt.Where(i => i.BookingId == booking.Id).ToList();
                bkdts.AddRange(dt);
            }
            var rooms = new List<Room>();
            foreach (var booking in bkdts)
            {
                var r = _context.Rooms.Find(booking.RoomId);
                if (!rooms.Any(i => i.Id == r.Id)) rooms.Add(r);
                rooms.Add(r);
            }
            var models = new List<RoomViewModel>();
            foreach (var room in list)
            {
                var model = new RoomViewModel
                {
                    Id = room.Id,
                    Name = room.Name,
                };
                if (rooms.Any(i => i.Id == model.Id))
                    model.Status = "Đã đặt";
                else model.Status = "Trống";
                models.Add(model);
            }
            return View(models);
        }
        [HttpPost]
        public IActionResult RoomStatus(int time)
        {
            DateTime begin, end;
            switch (time)
            {               
                case 2:
                    begin = DateTime.Now.Date.AddHours(6);
                    end = DateTime.Now.Date.AddHours(12);
                    break;
                case 3:
                    begin = DateTime.Now.Date.AddHours(12);
                    end = DateTime.Now.Date.AddHours(18);
                    break;
                case 4:
                    begin = DateTime.Now.Date.AddHours(18);
                    end = DateTime.Now.Date.AddHours(24);
                    break;
                default: begin = DateTime.Now; end = DateTime.Now; break;
            }
            return RedirectToAction ("RoomStatus", new {begin = begin, end = end});
        }
        public IActionResult RoomDetail(int id)
        {
            var model = _context.Rooms.Find(id);
            return View(model);
        }
    }
}
