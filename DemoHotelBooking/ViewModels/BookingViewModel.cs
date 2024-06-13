using DemoHotelBooking.Models;
using System.ComponentModel.DataAnnotations;

namespace DemoHotelBooking.ViewModels
{
    public class BookingViewModel
    {
        [Phone, Required]
        public string? Phone { get; set; } // Id khách hàng

        [Required]
        public string? Name { get; set; }

        public double Deposit { get; set; } //tiền cọc

        public DateTime CheckinDate { get; set; } //ngày nhận dự kiến

        public DateTime CheckoutDate { get; set; } // ngày trả dự kiến

        public double? Amount { get; set; } //chi phí tổng

        public List<Room> Rooms { get; set; }
        public BookingViewModel()
        {
            CheckinDate = DateTime.Now;
            CheckoutDate = DateTime.Now;
        }
    }
}
