namespace DemoHotelBooking.Models
{
    public enum PAYMETHOD
    { 
        CASH = 0,
        VNPAY = 1
    }
    public enum BOOKINGSTATUS
    { 
        NONE = 0,
        DEPOSITED = 1,
        SUCCESS = 2,
        CHANGED = 3,
        CANCELLED = 4
    }
    public class Booking
    {
        public int Id { get; set; }

        public string? CusID { get; set; } // Id khách hàng

        public AppUser Customer { get; set; }

        public int Paymethod { get; set; } //phương thức đặt cọc

        public double Deposit { get; set; } //tiền cọc

        public DateTime CreateDate { get; set; } //ngày tạo đơn

        public DateTime CheckinDate { get; set; } //ngày nhận dự kiến

        public DateTime CheckoutDate { get; set; } // ngày trả dự kiến

        public int? Status { get; set; } // trạng thái đặt phòng

        public double? Amount { get; set; } //chi phí tổng


        //hàm tính ngày thuê
        public int SoNgayThue(DateTime startDate, DateTime endDate)
        {
            return endDate.Day - startDate.Day;
        }
        //hàm tính giờ thuê
        public int SoGioThue(DateTime startDate, DateTime endDate)
        {
            int start = startDate.Hour, end = endDate.Hour;
            if (start > end) // qua đêm
                return 12;
            return end - start;
        }
    }
}