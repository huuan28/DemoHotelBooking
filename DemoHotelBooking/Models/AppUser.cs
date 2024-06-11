using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace DemoHotelBooking.Models
{
    public class AppUser : IdentityUser
    {

        [Required]
        [Display(Name ="Họ và tên")]
        public string FullName { get; set; }

        public string SearchKey { get; set; } //Từ khóa tìm kiếm tài khoản (KH.../NV...)

        public bool IsRegisted { get; set; } //đã đăng ký hay chưa (để đánh dấu khách vãng lai)


        //Hàm này để sinh mã tìm kiếm của tài khoản
        public static string GenerateSearchKey(string role)
        {
            string max = GetMaxNum(role);
            switch (role)
            {
                case "Customer":
                    return "KH" + max;
                case "Receptionist":
                    return "NV" + max;
            }
            return "";
        }
        private static string GetMaxNum(string role)
        {
            return "000";
        }
    }
}
