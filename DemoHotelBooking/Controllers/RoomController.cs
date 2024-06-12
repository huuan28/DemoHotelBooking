using Microsoft.AspNetCore.Mvc;

namespace DemoHotelBooking.Controllers
{
    public class RoomController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
