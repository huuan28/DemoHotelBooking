using Microsoft.AspNetCore.Mvc;

namespace DemoHotelBooking.Controllers
{
    public class InvoiceController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
