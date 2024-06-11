using System.ComponentModel.DataAnnotations;

namespace DemoHotelBooking.Models
{
    public enum FeedBackState
    {
        Show = 0,
        Hide = 1
    }
    public class Feedback
    {
        [Key]
        public int Id { get; set; }

        public string CusId { get; set; }
        public AppUser User { get; set; }

        public int StarNumber { get; set; }
        public string Content { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime EditDate { get; set; }

        public int Status { get; set; }
    }
}
