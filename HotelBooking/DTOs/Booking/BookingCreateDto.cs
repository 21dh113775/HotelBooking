namespace HotelBooking.DTOs.Booking
{
    public class BookingCreateDto
    {
        public int RoomId { get; set; }
        public int? ComboId { get; set; }
        public int? UserId { get; set; } // hoặc lấy từ JWT
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public List<int>? DrinkIds { get; set; }
    }

}
