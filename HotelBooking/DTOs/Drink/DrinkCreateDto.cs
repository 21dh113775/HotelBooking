namespace HotelBooking.DTOs.Drink
{
    public class DrinkCreateDto
    {
        public string Name { get; set; } = null!;
        public decimal Price { get; set; }
        public string? ImageUrl { get; set; }
    }
}
