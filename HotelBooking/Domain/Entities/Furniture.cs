using Microsoft.EntityFrameworkCore;

namespace HotelBooking.Domain.Entities
{
    public class Furniture
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        [Precision(18, 2)]
        public decimal Price { get; set; }
        public string ImageUrl { get; set; }

        public ICollection<RoomFurniture> RoomFurnitures { get; set; }
    }


}
