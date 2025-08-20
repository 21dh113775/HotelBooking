using Microsoft.AspNetCore.Http;

namespace HotelBooking.DTOs.Room
{
    public class RoomUpdateDto
    {
        public string Name { get; set; }
        public string? Description { get; set; }
        public decimal PricePerHour { get; set; }
        public IFormFile? Image { get; set; }
    }
}
