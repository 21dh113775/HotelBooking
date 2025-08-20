namespace HotelBooking.Domain.Entities
{
    public class Payment
    {
        public int Id { get; set; }

        public int BookingId { get; set; }
        public Booking Booking { get; set; }

        public string Method { get; set; } // Cash / Banking / MoMo / ZaloPay
        public string Status { get; set; } // Paid / Unpaid / Failed
        public DateTime? PaidAt { get; set; }
    }

}
