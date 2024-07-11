using DemoHotelBooking.Models;
using System.ComponentModel.DataAnnotations;

namespace DemoHotelBooking.ViewModels
{
    public class BookingViewModel
    {
        [Phone, Required]
        public string? Phone { get; set; } // sdt khách hàng

        [Required]
        public string? Name { get; set; }

        public double? Deposit { get; set; } //tiền cọc

        public DateTime CheckinDate { get; set; } //ngày nhận dự kiến

        public DateTime CheckoutDate { get; set; } // ngày trả dự kiến

        public double? Amount { get; set; } //chi phí tổng

        public List<Room> SelectedRooms { get; set; }

        public List<Room> AvailbleRooms { get; set; }

        public AppUser? Customer { get; set; }

        public BookingViewModel()
        {
            SelectedRooms = new List<Room>();
            AvailbleRooms = new List<Room>();
            CheckinDate = DateTime.Now;
            CheckoutDate = DateTime.Now;
        }
    }
}
