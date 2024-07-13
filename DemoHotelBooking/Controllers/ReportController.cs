using DemoHotelBooking.Models;
using DemoHotelBooking.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DemoHotelBooking.Controllers
{
    [Authorize(Roles ="Accountant")]
    public class ReportController : Controller
    {
        private readonly AppDbContext _context;
        private readonly UserManager<AppUser> _userManager;
        public ReportController(AppDbContext context, UserManager<AppUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult ReportList()
        {
            var model = _context.ReportRevenues.Include(a=>a.Accountant).OrderByDescending(i=>i.Id).ToList();
            var rps = new ReportRevenue[12];
            double[] data = new double[12];
            for (int i = 0; i < 12; i++)
            {
                rps[i] = _context.ReportRevenues.Where(a => a.Date.Month == i + 1).OrderByDescending(x => x.Date).FirstOrDefault();
                if (rps[i] != null)
                    data[i] = rps[i].Total;
            }
            var dt = new Statistic
            {
                Data = data,
            };
            ViewData["Statistic"] = dt;
            return View(model);
        }
        //[Authorize(Roles ="Accountant")]
        public async Task<IActionResult> Create()
        {
            var Accountant = await _userManager.GetUserAsync(HttpContext.User);
            if (!User.IsInRole("Accountant"))
            {
                return RedirectToAction("ReportList");
            }
            var rp = _context.ReportRevenues.FirstOrDefault(i => i.Date.Month == DateTime.Today.Month && i.Date.Year == DateTime.Today.Year);
            //if (rp != null)
            //{
            //    ViewBag.Error = "Báo cáo đã tồn tại";
            //    return RedirectToAction("ReportList");
            //}
            rp = new ReportRevenue
            {
                Date = DateTime.Now,
                AccId = Accountant.Id,
                Total = 0
            };
            _context.ReportRevenues.Add(rp);
            _context.SaveChanges();
            var ivs = _context.Invoices.Where(i => i.PaymentDate.Month == DateTime.Today.Month && i.Status == 3).Include(z => z.Booking).ToList();
            var roomReport = new Dictionary<string, double>();
            var rooms = _context.Rooms.ToList();
            foreach (var r in rooms)
            {
                roomReport[r.Id.ToString()] = 0;               
            }
            foreach (var i in ivs)
            {
                rp.Total += i.Amount;
                var roomlist = _context.InvoiceDetails.Where(x => x.InvoiceId == i.Id).Include(a => a.Room).ToList();
                TimeSpan stayDuration = i.Booking.CheckoutDate - i.Booking.CheckinDate;
                int numberOfDays = stayDuration.Days;
                foreach (var j in roomlist)
                {
                    roomReport[j.RoomId.ToString()] += (j.Price+j.Price*j.SubFee/100) * numberOfDays;
                }
            }
            _context.ReportRevenues.Update(rp);
            _context.SaveChanges();
            foreach (var r in roomReport)
            {              
                var dpdt = new ReportDetail
                {
                    ReportId = rp.Id,
                    RoomId = int.Parse(r.Key),
                    RoomRevuenue = r.Value,
                };
                _context.ReportDetails.Add(dpdt);
            }
            _context.SaveChanges();
            var rpv = new ReportView
            {
                ReportRevenue = rp
            };
            rpv.ReportDetails = new List<ReportDetailView>();
            var rpdt = _context.ReportDetails.Where(i => i.ReportId == rp.Id).ToList();
            foreach (var r in rpdt)
            {
                r.RoomRevuenue = roomReport[r.RoomId.ToString()];
                var rpdtv = new ReportDetailView
                {
                    ReportDetail = r,                   
                };
                rpv.ReportDetails.Add(rpdtv);
            }
            return RedirectToAction("Details", new { id = rp.Id });
        }
        public IActionResult Details(int id)
        {
            var rp = _context.ReportRevenues.Include(i=>i.Accountant).FirstOrDefault(a=>a.Id==id);
            var rpv = new ReportView
            {
                ReportRevenue = rp
            };
            rpv.ReportDetails = new List<ReportDetailView>();
            var rpdt = _context.ReportDetails.Where(i => i.ReportId == rp.Id).Include(z=>z.Room).ToList();
            foreach (var r in rpdt)
            {
                var rpdtv = new ReportDetailView
                {
                    ReportDetail = r,
                };
                rpdtv.Percent = string.Format("{0:f2}",(rpdtv.ReportDetail.RoomRevuenue / rp.Total * 100));
                rpv.ReportDetails.Add(rpdtv);
            }            
            return View(rpv);
        }
        public IActionResult Statistic()
        {
            var rps = new ReportRevenue[12];
            double[] data = new double[12];
            for (int i = 0; i < 12; i++)
            {
                rps[i] = _context.ReportRevenues.Where(a => a.Date.Month == i+1).OrderByDescending(x => x.Date).FirstOrDefault();
                if (rps[i] != null)
                    data[i] = rps[i].Total;
            }
            var dt = new Statistic
            {
                Data = data,
            };
            return View(dt);
        }
    }
}
