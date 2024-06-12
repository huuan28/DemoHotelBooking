using Microsoft.AspNetCore.Mvc;

namespace DemoHotelBooking.Controllers
{
    public class FeedBackController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
