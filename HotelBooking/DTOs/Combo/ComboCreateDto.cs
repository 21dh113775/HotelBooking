namespace HotelBooking.DTOs.Combo
{
    public class ComboCreateDto
    {
        public string Name { get; set; }
        public decimal TotalPrice { get; set; }
        public List<int> FurnitureIds { get; set; }
        public List<int> RoomIds { get; set; }
    }

}
