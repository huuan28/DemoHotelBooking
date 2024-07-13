using DemoHotelBooking.Models;
using DemoHotelBooking.Services;
using DemoHotelBooking.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Collections.Generic;


namespace DemoHotelBooking.Controllers
{
    public class BookingController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly AppDbContext _context;
        private readonly IVnPayService _vnPayService;

        private BookingViewModel currentBooking;
        private AppUser currentUser;
        public BookingController(AppDbContext context, UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, RoleManager<IdentityRole> roleManager, IVnPayService service)
        {
            _vnPayService = service;
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
        }
        private Task<AppUser> GetCurrentUserAsync() => _userManager.GetUserAsync(HttpContext.User);

        //Lấy thông tin đặt phòng từ session
        private BookingViewModel GetBookingFromSession()
        {
            var bookingJson = HttpContext.Session.GetString("CurrentBooking");
            if (string.IsNullOrEmpty(bookingJson))
            {
                return new BookingViewModel { CheckinDate = DateTime.Now };
            }
            return JsonConvert.DeserializeObject<BookingViewModel>(bookingJson);
        }
        //Lưu thông tin đặt phòng vào session
        private void SaveBookingToSession(BookingViewModel booking)
        {
            var bookingJson = JsonConvert.SerializeObject(booking);
            HttpContext.Session.SetString("CurrentBooking", bookingJson);
        }

        //Đặt phòng
        [HttpGet]
        public async Task<IActionResult> Booking(int? id)
        {
            currentBooking = GetBookingFromSession();
            UpDateAvailbleRooms();
            currentUser = await GetCurrentUserAsync();
            if (currentUser != null)
            {
                currentBooking.Phone = currentUser.PhoneNumber;
                currentBooking.Name = currentUser.FullName;
            }
            if (id != null)
            {
                var room = _context.Rooms.FirstOrDefault(r => r.Id == id);
                currentBooking.SelectedRooms.Add(room);
            }
            ViewData["availbleRooms"] = currentBooking.AvailbleRooms;
            ViewData["bookingRooms"] = currentBooking.SelectedRooms;
            SaveBookingToSession(currentBooking);
            return View(currentBooking);
        }
        [HttpPost]
        public async Task<IActionResult> Booking(BookingViewModel model)
        {
            if (ModelState.IsValid)
            {
                TimeSpan stayDuration = model.CheckoutDate - model.CheckinDate;
                int numberOfDays = stayDuration.Days;
                var user = _context.Users.FirstOrDefault(i => i.PhoneNumber == model.Phone);
                //Kiểm tra đã đăng ký chưa
                if (user == null)
                {
                    if (!await CreateUnRegisterUser(model.Phone, model.Name))
                        return View(currentBooking); //lưu tài khoản loại chưa đăng ký
                    user = await _userManager.FindByNameAsync(model.Phone);
                }
                //if (!user.IsRegisted)
                //{
                //    user.FullName = model.Name;
                //    _context.Users.Add(user);
                //}
                currentBooking = GetBookingFromSession();
                if (currentBooking.SelectedRooms.Count == 0)
                {
                    ViewBag.Error = "Chưa chọn phòng!!!";
                    ViewData["availbleRooms"] = currentBooking.AvailbleRooms;
                    ViewData["bookingRooms"] = currentBooking.SelectedRooms;
                    return View(model);
                }
                if (model.CheckinDate < DateTime.Now)
                {
                    ViewBag.Error = "Không thể nhận/trả phòng ở thời điểm này!!!";
                    ViewData["availbleRooms"] = currentBooking.AvailbleRooms;
                    ViewData["bookingRooms"] = currentBooking.SelectedRooms;
                    return View(model);
                }
                if (model.CheckinDate >= model.CheckoutDate || model.CheckoutDate.Day - model.CheckinDate.Day < 1)
                {
                    ViewBag.Error = "Cần đặt ít nhất 1 ngày!!!";
                    ViewData["availbleRooms"] = currentBooking.AvailbleRooms;
                    ViewData["bookingRooms"] = currentBooking.SelectedRooms;
                    return View(model);
                }
                currentBooking.CheckinDate = model.CheckinDate;
                currentBooking.CheckinDate = model.CheckinDate;
                currentBooking.Name = model.Name;
                currentBooking.Phone = model.Phone;
                currentBooking.Deposit = currentBooking.SelectedRooms.Sum(i => i.Price) * 0.2 * numberOfDays;
                currentBooking.Amount = currentBooking.SelectedRooms.Sum(i => i.Price) * numberOfDays;
                currentBooking.Customer = user;
                SaveBookingToSession(currentBooking);
                var vnPayModel = new VnPaymentRequestModel()
                {
                    Amount = (double)currentBooking.Deposit,
                    CreateDate = DateTime.Now,
                    Description = $"{model.Phone}-{model.Name}",
                    FullName = model.Name,
                    BookingId = new Random().Next(1, 1000)
                };
                return Redirect(_vnPayService.CreatePaymentUrl(HttpContext, vnPayModel, "a"));
            }
            ViewData["availbleRooms"] = currentBooking.AvailbleRooms;
            ViewData["bookingRooms"] = currentBooking.SelectedRooms;
            return View(model);

        }
        //Chọn phòng
        public IActionResult AddRoom(int Id)
        {
            currentBooking = GetBookingFromSession();
            var room = _context.Rooms.Find(Id);
            if (!currentBooking.SelectedRooms.Any(i => i.Id == Id))
                currentBooking.SelectedRooms.Add(room);
            ViewData["availbleRooms"] = currentBooking.AvailbleRooms;
            ViewData["bookingRooms"] = currentBooking.SelectedRooms;
            SaveBookingToSession(currentBooking);
            return PartialView("BookingRooms", currentBooking.SelectedRooms);
        }
        //Bỏ chọn phòng
        public IActionResult RemoveRoom(int id)
        {
            currentBooking = GetBookingFromSession();
            var room = currentBooking.SelectedRooms.FirstOrDefault(i => i.Id == id);
            if (room == null)
                return NotFound();
            currentBooking.SelectedRooms.Remove(room);
            SaveBookingToSession(currentBooking);
            ViewData["availbleRooms"] = currentBooking.AvailbleRooms;
            ViewData["bookingRooms"] = currentBooking.SelectedRooms;
            return PartialView("BookingRooms", currentBooking.SelectedRooms);
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
            currentBooking = GetBookingFromSession();
            currentBooking.CheckinDate = start;
            currentBooking.CheckoutDate = end;
            UpDateAvailbleRooms();
            ViewData["availbleRooms"] = currentBooking.AvailbleRooms;
            ViewData["bookingRooms"] = currentBooking.SelectedRooms;
            return PartialView("ListRoomAvailble", currentBooking.AvailbleRooms);
        }
        //cập nhật phòng trống
        public void UpDateAvailbleRooms()
        {
            var rooms = _context.Rooms.ToList();
            currentBooking.AvailbleRooms.Clear();
            foreach (var room in rooms)
            {
                if (RoomIsAvailble(room.Id, currentBooking.CheckinDate, currentBooking.CheckoutDate))
                { currentBooking.AvailbleRooms.Add(room); }
            }
            SaveBookingToSession(currentBooking);
        }
        //tạo tài khoản cho người chưa đăng ký
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
                // Gán vai trò "Customer" cho người dùng
                await _userManager.AddToRoleAsync(user, "Customer");

                return true;
            }
            return false;
        }


        //kết quả trả về của VNPAY
        public async Task<IActionResult> PaymentCallBack()
        {
            // Lấy thông tin từ query string của VnPay để xác thực và cập nhật trạng thái đơn hàng
            var response = _vnPayService.PaymentExecute(Request.Query);
            if (response == null || response.VnPayResponseCode != "00")
            {
                //thanh toán thất bại
                return RedirectToAction("PaymentFail");
            }

            //lấy thông tin đặt phòng từ viewmodel
            currentBooking = GetBookingFromSession();

            //lấy thông tin khách hàng
            // tạo mới đơn đặt phòng
            var booking = new Booking
            {
                CreateDate = DateTime.Now,
                CheckinDate = currentBooking.CheckinDate,
                CheckoutDate = currentBooking.CheckoutDate,
                Deposit = (double)currentBooking.Deposit,
                CusID = currentBooking.Customer.Id,
                Status = 1
            };
            //lưu vào DB
            _context.Bookings.Add(booking);
            await _context.SaveChangesAsync();

            //thêm và lưu danh sách phòng đã chọn
            foreach (var room in currentBooking.SelectedRooms)
            {
                var detail = new BookingDetail
                {
                    BookingId = booking.Id,
                    RoomId = room.Id,
                    Price = room.Price
                };
                _context.BookingDetails.Add(detail);
                await _context.SaveChangesAsync();
            }
            //xóa viewmodel
            HttpContext.Session.Remove("CurrentBooking");

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

        public async Task<IActionResult> History()
        {
            var cus = await GetCurrentUserAsync();
            var bks = _context.Bookings.Where(i => i.CusID == cus.Id).Include(i => i.Customer).ToList();
            var models = new List<BookingView>();
            foreach (var i in bks)
            {
                models.Add(new BookingView
                {
                    Booking = i
                });
            }
            return View(models);
        }
        public IActionResult BookingDetails(int id)
        {
            var bkdt = _context.Bookings.Find(id);
            var dt = _context.BookingDetails.Where(i => i.BookingId == id).Include(i => i.Room).ToList();
            var model = new BookingView
            {
                Booking = bkdt,
                Rooms = dt
            };
            return View(model);
        }
        public async Task<IActionResult> CancelBooking(int id)
        {
            var cus = await GetCurrentUserAsync();
            if (cus == null) return NotFound();
            var bk = _context.Bookings.Find(id);
            if (bk == null) return NotFound();
            bk.Status =3;
            _context.Bookings.Update(bk);
            _context.SaveChanges();
            return RedirectToAction("BookingDetails",new {id=id});
        }
    }
}