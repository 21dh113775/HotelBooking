namespace HotelBooking.Domain.Entities
{
    public class ComboDetail
    {
        public int Id { get; set; }

        public int ComboId { get; set; }
        public Combo Combo { get; set; }

        public int? RoomId { get; set; }
        public Room? Room { get; set; }

        public int? FurnitureId { get; set; }
        public Furniture? Furniture { get; set; }
    }

}
