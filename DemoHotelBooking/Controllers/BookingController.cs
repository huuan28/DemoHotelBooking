using DemoHotelBooking.Models;
using DemoHotelBooking.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;


namespace DemoHotelBooking.Controllers
{
    public class BookingController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly AppDbContext _context;

        private static List<Room> bookingRooms = new List<Room>();
        private static List<Room> availbleRooms = new List<Room>();
        private static Booking currentBooking;
        private DateTime start;
        private DateTime end;
        private static AppUser currentUser;
        public BookingController(AppDbContext context, UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, RoleManager<IdentityRole> roleManager)
        {
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            currentBooking = new Booking();
            availbleRooms = GetRooms(currentBooking.CheckinDate, currentBooking.CheckoutDate);
        }
        private Task<AppUser> GetCurrentUserAsync() => _userManager.GetUserAsync(HttpContext.User);
        public List<Room> GetRooms(DateTime start, DateTime end)
        {
            var rooms = _context.Rooms.ToList();
            var list = new List<Room>();
            foreach (var room in rooms)
            {
                if (RoomIsAvailble(room.Id, start, end))
                { list.Add(room); }
            }
            return list;
        }
        public async Task<IActionResult> Booking(int? id)
        {
            ViewData["availbleRooms"] = availbleRooms;
            ViewData["bookingRooms"] = bookingRooms;
            currentUser = await GetCurrentUserAsync();
            if (currentUser == null)
                return View();
            BookingViewModel viewModel = new BookingViewModel
            {
                Phone = currentUser.PhoneNumber,
                Name = currentUser.FullName
            };
            return View(viewModel);
        }
        [HttpPost]
        public async Task<IActionResult> Booking(BookingViewModel model)
        {

            if (ModelState.IsValid)
            {
                bool a = HttpContext.User.Identity.IsAuthenticated;
                //_context.Bookings.Add(model);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(model);
        }
        public IActionResult AddRoom(int Id)
        {
            var room = _context.Rooms.Find(Id);
            if (!bookingRooms.Any(i => i.Id == Id))
                bookingRooms.Add(room);
            ViewData["availbleRooms"] = availbleRooms;
            ViewData["bookingRooms"] = bookingRooms;
            return PartialView("BookingRooms", bookingRooms);
        }
        public IActionResult RemoveRoom(int id)
        {
            var room = bookingRooms.FirstOrDefault(i => i.Id == id);
            if (room == null)
                return NotFound();
            bookingRooms.Remove(room);
            ViewData["availbleRooms"] = availbleRooms;
            ViewData["bookingRooms"] = bookingRooms;
            return PartialView("BookingRooms", bookingRooms);
        }
        //Kiểm tra lịch
        public bool RoomIsAvailble(int roomId, DateTime startDate, DateTime endDate)
        {
            var bookings = _context.Bookings
                .Where(i =>
                    (i.CheckinDate <= startDate && i.CheckoutDate >= startDate) ||
                    (i.CheckinDate <= endDate && i.CheckoutDate >= endDate) ||
                    (i.CheckinDate >= startDate && i.CheckoutDate <= endDate))
                .ToList();
            foreach (var booking in bookings)
            {
                bool flag = _context.BookingDetails.Any(i => i.RoomId == roomId);
                if (flag)
                    return false;
            }
            return true;
        }
        [HttpPost]
        public IActionResult UpdateTime(DateTime start, DateTime end)
        {
            currentBooking.CheckinDate = start;
            currentBooking.CheckoutDate = end;
            //return Json(new {success = true});
            availbleRooms = GetRooms(start, end);
            ViewData["availbleRooms"] = availbleRooms;
            ViewData["bookingRooms"] = bookingRooms;
            return PartialView("ListRoomAvailble", availbleRooms);
        }
    }
}
