using HotelBooking.Modules.Entity_Models;

namespace HotelBooking.Domain.Entities
{
    public class Booking
    {
        public int Id { get; set; }

        public int UserId { get; set; }
        public User User { get; set; }

        public int RoomId { get; set; }
        public Room Room { get; set; }

        public int? ComboId { get; set; }
        public Combo? Combo { get; set; }

        public DateTime BookingTime { get; set; }
        public DateTime CheckIn { get; set; }
        public DateTime CheckOut { get; set; }
        public decimal TotalPrice { get; set; }
        public string Status { get; set; } // Pending, Confirmed, Cancelled

        public int? CreatedBy { get; set; } // Admin booking offline

        public ICollection<BookingDrink> BookingDrinks { get; set; }
        public Payment? Payment { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    }

}
