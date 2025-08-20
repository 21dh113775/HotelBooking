namespace HotelBooking.Domain.Entities
{
    public class BookingDrink
    {
        public int Id { get; set; }

        public int BookingId { get; set; }
        public Booking Booking { get; set; }

        public int DrinkId { get; set; }
        public Drink Drink { get; set; }

        public int Quantity { get; set; }
    }

}
