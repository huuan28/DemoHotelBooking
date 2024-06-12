using System.ComponentModel.DataAnnotations;

namespace DemoHotelBooking.Models
{
    public class Room
    {
        [Key]
        [Display(Name ="Mã số phòng")]
        public string Id { get; set; } //mã phòng (STD..., SUP..., DLX..., SUT) 

        [Required]
        [Display(Name = "Loại phòng")]
        public string Type { get; set; } //loại phòng (Standart, Superio, Deluxe, Suite)

        [Range(1,100)]
        [Display(Name = "Lầu")]
        public int FloorNumber { get; set; } //Số lầu

        [Required]
        [Display(Name = "Giá phòng")]
        public double Price { get; set; }

        public string? ImagePath { get; set; } //đường dẫn ảnh

        [Display(Name = "Giới thiệu")]
        public string? Introduce { get; set; } //nội dung giới thiệu
        [Display(Name = "Chi tiết phòng")]
        public string? Description { get; set; } //Mô tả

        [Display(Name = "Khung cảnh")]
        public string? Visio { get; set; } // View núi, view biển

        [Required]
        [Display(Name = "Số người qui định")]
        public int DAP { get; set; } // Default Amount of people (Số người mặc định)
        [Required]
        [Display(Name = "Số người tối đa")]
        public int MAP { get; set; } // Maximum Amount of people (Số người tối đa)
    }
}
