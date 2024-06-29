using System.ComponentModel.DataAnnotations;

namespace DemoHotelBooking.Models
{
    public class ReportDetail
    {
        public int ReportId { get; set; }
        public ReportRevenue ReportRevenue { get; set; }

        public int RoomId { get; set; }
        public Room Room { get; set; }

        public double RoomRevuenue { get; set; } // doanh thu phòng
    }
}
