using System.ComponentModel.DataAnnotations;

namespace DemoHotelBooking.Models
{
    public class RevenueDetail
    {
        [Key]
        public int Id { get; set; }

        public int RevenueId { get; set; }
        public Revenue Revenue { get; set; }

        public int RoomID { get; set; }
        public Room Room { get; set; }

        public double RevenueOfRoom { get; set; } // doanh thu phòng
    }
}
