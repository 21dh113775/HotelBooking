using HotelBooking.Modules.Entity_Models;

namespace HotelBooking.Domain.Entities
{
    public class StaffShift
    {
        public int Id { get; set; }

        public int StaffId { get; set; }
        public User Staff { get; set; }

        public DateTime ShiftDate { get; set; }
        public string ShiftTime { get; set; } // Morning / Afternoon / Night
    }

}
