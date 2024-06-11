using System.ComponentModel.DataAnnotations;

namespace DemoHotelBooking.Models
{
    public class RoomImage
    {
        [Key]
        public int Id { get; set; }

        public string RoomId { get; set; }
        public Room Room { get; set; }

        public string ImageName { get; set; }
        public bool IsDefault { get; set; } //Ảnh mặc định
    }
}
