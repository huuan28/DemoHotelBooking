using DemoHotelBooking.Models;
using DemoHotelBooking.ViewModels;
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
            return View(new RoomViewModel());
        }
        [HttpPost]
        public async Task<IActionResult> Create(RoomViewModel model)
        {
            if (ModelState.IsValid)
            {
                Room room = _context.Rooms.FirstOrDefault(i=> i.Id == model.Id);
                if (room == null)
                {
                    room = new Room();
                    room.Name = model.Name;
                    room.Type = model.Type;
                    room.FloorNumber = model.FloorNumber;
                    room.ImagePath = model.ImagePath;
                    room.Introduce = model.Introduce;
                    room.Description = model.Description;
                    room.MAP = model.MAP;
                    room.DAP = model.DAP;
                    room.Visio = model.Visio;
                    room.Price = model.Price;
                    _context.Rooms.Add(room);
                    await _context.SaveChangesAsync();
                    return RedirectToAction("AllRoomList", "RoomManager", new {area = "Admin"});
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

        public IActionResult Update(int Id)
        {
            var room = _context.Rooms.FirstOrDefault(r => r.Id == Id);
            if (room == null)
                return NotFound();
            return View(room);
        }
        [HttpPost]
        public async Task<IActionResult> Update(RoomViewModel model)
        {
            if (ModelState.IsValid)
            {
                var room = _context.Rooms.FirstOrDefault(i => i.Id == model.Id);
                if(room != null)
                {
                    room.Name = model.Name;
                    room.Type = model.Type;
                    room.FloorNumber = model.FloorNumber;
                    room.ImagePath = model.ImagePath;
                    room.Introduce= model.Introduce;
                    room.Description = model.Description;
                    room.MAP = model.MAP;
                    room.DAP = model.DAP;
                    room.Visio = model.Visio;
                    room.Price = model.Price;
                    _context.Rooms.Update(room);
                    await _context.SaveChangesAsync();
                    return RedirectToAction("AllRoomList", "RoomManager", new { area = "Admin" });
                }
            }
            ViewBag.Error = "Thông tin không hợp lệ";
            return View(model);
        }
        public async Task<IActionResult> Delete(int id)
        {
            var room = _context.Rooms.FirstOrDefault(i=> i.Id==id);
            if(room != null)
            {
                _context.Rooms.Remove(room);
                await _context.SaveChangesAsync();
                return RedirectToAction("AllRoomList", "RoomManager", new { area = "Admin" });
            }
            return NotFound();
        }

    }
}
