namespace HotelBooking.Domain.Entities
{
    public class RoomFurniture
    {
        public int RoomId { get; set; }
        public Room Room { get; set; }

        public int FurnitureId { get; set; }
        public Furniture Furniture { get; set; }
    }

}
