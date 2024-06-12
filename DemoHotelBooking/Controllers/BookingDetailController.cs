using Microsoft.AspNetCore.Mvc;

namespace DemoHotelBooking.Controllers
{
    public class BookingDetailController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
