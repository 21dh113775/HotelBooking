namespace HotelBooking.DTOs
{
    public class StaffCreateDto
    {
        public string FullName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Password { get; set; }
        public string Role { get; set; } = "Staff"; // Mặc định Staff, có thể Manager
    }
}