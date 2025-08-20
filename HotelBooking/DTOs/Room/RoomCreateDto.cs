using Microsoft.AspNetCore.Http;

namespace HotelBooking.DTOs
{
    public class RoomCreateDto
    {
        public string RoomNumber { get; set; } = string.Empty;
        public double PricePerNight { get; set; }
        public bool IsAvailable { get; set; }
        public string? Description { get; set; }
        public IFormFile? Image { get; set; }
    }
}