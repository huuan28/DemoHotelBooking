using System.ComponentModel.DataAnnotations;

namespace DemoHotelBooking.Models
{
    public class InvoiceDetail
    {
        [Key]
        public int Id { get; set; }

        public int InvoiceId { get; set; }
        public Invoice Invoice { get; set; }

        public int RoomId { get; set; }
        public Room Room { get; set; }

        public DateTime CheckoutDate { get; set; }
        public double Price { get; set; }
        public int PeopleNumber { get; set; } //số người
        public int SurchageRate { get; set; } //tỷ lệ phụ thu
        public string Note { get; set; }
    }
}
