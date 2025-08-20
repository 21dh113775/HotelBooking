namespace HotelBooking.Domain.Entities
{
    public class Voucher
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public int Discount { get; set; } // %
        public DateTime ExpiryDate { get; set; }
        public bool IsActive { get; set; }
    }

}
