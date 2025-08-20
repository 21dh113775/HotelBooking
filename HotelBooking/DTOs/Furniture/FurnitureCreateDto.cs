using System.ComponentModel.DataAnnotations;

namespace HotelBooking.DTOs.Furniture
{
    public class FurnitureCreateDto
    {
        public string Name { get; set; }
        public decimal Price { get; set; }
        public IFormFile? Image { get; set; }
    }



}
