using HotelBooking.Domain.Entities;
using HotelBooking.Modules.Entity_Models;

public class Booking
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public int RoomId { get; set; }
    public int? ComboId { get; set; }
    public DateTime BookingTime { get; set; }
    public DateTime CheckIn { get; set; }
    public DateTime CheckOut { get; set; }
    public decimal TotalPrice { get; set; }
    public string Status { get; set; } = "Pending";
    public int CreatedBy { get; set; } // Thay int? thành int
    public DateTime CreatedAt { get; set; }
    public User User { get; set; }
    public Room Room { get; set; }
    public Combo? Combo { get; set; }
    public List<BookingDrink> BookingDrinks { get; set; }
}