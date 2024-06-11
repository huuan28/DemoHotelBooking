using System.ComponentModel.DataAnnotations;

namespace DemoHotelBooking.Models
{
    public class Revenue
    {
        [Key]
        public int Id { get; set; }
        public int Month { get; set; }
        public int Year { get; set; }
        public double Total { get; set; }
    }
}
