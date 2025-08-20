using System.ComponentModel.DataAnnotations;

namespace HotelBooking.DTOs.Rom
{
    public class RoomCreateDto
    {
        public string Name { get; set; }
        public string? Description { get; set; }
        public decimal PricePerHour { get; set; }
        public IFormFile? Image { get; set; }
    }


}
