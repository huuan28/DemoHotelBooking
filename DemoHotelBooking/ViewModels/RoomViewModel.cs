using System.ComponentModel.DataAnnotations;

namespace DemoHotelBooking.ViewModels
{
    public class RoomViewModel
    {
        public int? Id { get; set; }

        [Display(Name = "Mã phòng")]
        public string Name { get; set; } //mã phòng (STD..., SUP..., DLX..., SUT) 

        [Display(Name = "Loại phòng")]
        public string Type { get; set; } //loại phòng (Standart, Superio, Deluxe, Suite)

        [Range(1, 100)]
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

        public string? Status {  get; set; }
        public string? StatusColor
        {
            get
            {
                switch (Status)
                {
                    case "Đã đặt": return "bg-warning";
                    case "Bảo trì": return "bg-light";
                    case "Đang thuê": return "bg-danger";
                    default: return "bg-success";
                }
            }
        }
    }
}
