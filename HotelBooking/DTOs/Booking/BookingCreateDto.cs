namespace HotelBooking.DTOs.Booking
{
    public class BookingCreateDto
    {
        public int RoomId { get; set; }
        public int? ComboId { get; set; }
        public int? UserId { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public List<int>? DrinkIds { get; set; }
        public string? VoucherCode { get; set; } // === GIẢI THÍCH: Thêm trường mã voucher ===
        // Lý do: Cho phép áp dụng giảm giá khi tạo booking ===
    }
}   