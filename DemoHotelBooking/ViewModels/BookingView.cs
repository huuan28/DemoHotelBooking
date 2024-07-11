using DemoHotelBooking.Models;

namespace DemoHotelBooking.ViewModels
{
    public class BookingView
    {
        public Booking Booking { get; set; }
        public List<BookingDetail>? Rooms { get; set; }
        public string Status
        {
            get
            {
                switch (Booking.Status)
                {
                    case 1: return "Đã đặt cọc";
                    case 2: return "Đã đổi";
                    case 3: return "Đã hủy";
                    case 4: return "Đã nhận";
                    default: return "";
                }
            }
        }
        public string StatusColor
        {
            get
            {
                switch (Booking.Status)
                {
                    case 1: return "text-primary";
                    case 2: return "text-warning";
                    case 3: return "text-danger";
                    case 4: return "text-success";
                    default: return "";
                }
            }
        }

    }
}
