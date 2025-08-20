namespace HotelBooking.Domain.Entities
{
    public class Combo
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string ComboType { get; set; } // RoomOnly, FurnitureOnly, RoomFurniture
        public decimal Price { get; set; }

        public ICollection<ComboDetail> ComboDetails { get; set; }
    }

}
