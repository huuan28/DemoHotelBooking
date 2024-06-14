using DemoHotelBooking.Models;
using DemoHotelBooking.Services;
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
        private readonly IVnPayService _vnPayService;
        private static int bookingId;

        private static List<Room> bookingRooms = new List<Room>();
        private static List<Room> availbleRooms = new List<Room>();
        private static Booking currentBooking = new Booking();
        private DateTime start;
        private DateTime end;
        private static AppUser currentUser;
        public BookingController(AppDbContext context, UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, RoleManager<IdentityRole> roleManager, IVnPayService service)
        {
            _vnPayService = service;
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
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
        [HttpGet]
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
                TimeSpan stayDuration = model.CheckoutDate - model.CheckinDate;
                int numberOfDays = stayDuration.Days + 1;
                var user = _context.Users.FirstOrDefault(i => i.PhoneNumber == model.Phone);
                //Kiểm tra đăng nhập
                if (user == null)
                {
                    CreateUnRegisterUser(model.Phone, model.Name);
                    user = _context.Users.FirstOrDefault(i => i.PhoneNumber == model.Phone);
                }
                DateTime dateTime = DateTime.Now;
                currentBooking.CreateDate = dateTime;
                currentBooking.CheckinDate = model.CheckinDate;
                currentBooking.CheckoutDate = model.CheckoutDate;
                currentBooking.Deposit = bookingRooms.Sum(i => i.Price) * 0.2 * numberOfDays;
                currentBooking.Amount = bookingRooms.Sum(i => i.Price);
                currentBooking.CusID = user.Id;
                _context.Bookings.Add(currentBooking);
                await _context.SaveChangesAsync();

                currentBooking = _context.Bookings.FirstOrDefault(i => i.CusID == user.Id && i.CreateDate == dateTime);

                var vnPayModel = new VnPaymentRequestModel()
                {
                    Amount = (double)currentBooking.Deposit,
                    CreateDate = currentBooking.CreateDate,
                    Description = $"{model.Phone}-{model.Name}",
                    FullName = model.Name,
                    BookingId = new Random().Next(1, 1000)
                };
                return Redirect(_vnPayService.CreatePaymentUrl(HttpContext, vnPayModel));
            }
            ViewData["availbleRooms"] = availbleRooms;
            ViewData["bookingRooms"] = bookingRooms;
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
                bool flag = _context.BookingDetails.Any(i => i.RoomId == roomId && i.BookingId == booking.Id);
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

        public async Task<bool> CreateUnRegisterUser(string Phone, string FullName)
        {
            bool flag = _context.Users.Any(i => i.PhoneNumber == Phone);
            if (flag) return false;

            var user = new AppUser
            {
                UserName = Phone,
                FullName = FullName,
                IsRegisted = false,
                PhoneNumber = Phone
            };
            var result = await _userManager.CreateAsync(user, "Abcd@1234");
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
            }
            return true;
        }

        public async Task<IActionResult> PaymentCallBack()
        {

            var response = _vnPayService.PaymentExecute(Request.Query);
            if (response == null || response.VnPayResponseCode != "00")
            {
                return RedirectToAction("PaymentFail");
            }
            // Xử lý logic sau khi thanh toán hoàn tất tại đây

            //tạo chi tiết đặt phòng
            int id = currentBooking.Id;
            foreach (var room in bookingRooms)
            {
                var detail = new BookingDetail
                {
                    BookingId = currentBooking.Id,
                    RoomId = room.Id,
                    Price = room.Price
                };
                _context.BookingDetails.Add(detail);
                currentBooking.Status = 2;
                await _context.SaveChangesAsync();
            }
            // Lấy thông tin từ query string của VnPay để xác thực và cập nhật trạng thái đơn hàng
            return RedirectToAction("PaymentSuccess");
        }
        public IActionResult PaymentSuccess()
        {
            return View();
        }
        public IActionResult PaymentFail()
        {
            return View();
        }
    }
}