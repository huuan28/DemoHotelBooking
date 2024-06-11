using System.ComponentModel.DataAnnotations;

namespace DemoHotelBooking.Models
{
    public class BookingDetail
    {
        [Key]
        public int Id { get; set; }

        public int BookingId { get; set; }
        public Booking Booking { get; set; }

        public string RoomId { get; set; }
        public Room Room { get; set; }

        public double Price { get; set; }
    }
}
