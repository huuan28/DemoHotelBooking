using System.ComponentModel.DataAnnotations;

namespace DemoHotelBooking.Models
{
    public enum InvoiceState
    {
        None = 0,
        CheckedIn = 1, //đã nhận phòng
        CheckedOut = 2, //đã trả hết
        PaidOut = 3
    }
    public class Invoice
    {
        [Key]
        public int Id { get; set; }

        public int BookingId { get; set; } // mã đặt phòng
        public Booking Booking { get; set; }

        public DateTime CreateDate { get; set; } //thời gian tạo hóa đơn (Lập phiếu nhận)

        public DateTime PaymentDate { get; set; } //thời gian thanh toán

        public double Amount { get; set; } //tổng thanh toán

        public double Deposited { get; set; } //đã đặt cọc

        public int PayMethod { get; set; } // hình thức thanh toán

        public int Status { get; set; } //Trạng thái

    }
}
