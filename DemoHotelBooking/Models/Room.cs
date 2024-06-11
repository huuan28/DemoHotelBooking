using System.ComponentModel.DataAnnotations;

namespace DemoHotelBooking.Models
{
    public class Room
    {
        [Key]
        public string Id { get; set; } //mã phòng (STD..., SUP..., DLX..., SUT) 

        [Required]
        public string Type { get; set; } //loại phòng (Standart, Superio, Deluxe, Suite)

        [Range(1,100)]
        public int FloorNumber { get; set; } //Số lầu

        [Required]
        public double Price { get; set; }

        public string? ImagePath { get; set; } //đường dẫn ảnh

        public string? Introduce { get; set; } //nội dung giới thiệu

        public string? Description { get; set; } //Mô tả

        public string? Visio { get; set; } // View núi, view biển

        [Required]
        public int DAP { get; set; } // Default Amount of people (Số người mặc định)
        [Required]
        public int MAP { get; set; } // Maximum Amount of people (Số người tối đa)
    }
}
