using DemoHotelBooking.Models;
using Microsoft.AspNetCore.Mvc;

namespace DemoHotelBooking.Controllers
{
    public class InvoiceController : Controller
    {
        private readonly AppDbContext _context;
        public InvoiceController(AppDbContext context)
        {
            _context = context;
        }
        public IActionResult BookingList()
        {
            var list = _context.Bookings.ToList();
            foreach (var i in list)
            {
                i.Customer = _context.Users.Find(i.CusID);
            }
            return View(list);
        }
        public IActionResult BookingDetails(int id)
        {
            var bkdt = _context.Bookings.Find(id);
            return View(bkdt);
        }
    }
}
