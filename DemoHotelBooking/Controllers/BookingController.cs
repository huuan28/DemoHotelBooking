using Microsoft.AspNetCore.Mvc;

namespace DemoHotelBooking.Controllers
{
    public class BookingController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
