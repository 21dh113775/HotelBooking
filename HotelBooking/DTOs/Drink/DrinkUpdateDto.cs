namespace HotelBooking.DTOs.Drink
{
    public class DrinkUpdateDto
    {
        public string Name { get; set; } = null!;
        public decimal Price { get; set; }
        public string? ImageUrl { get; set; }
    }
}
