using System;

namespace HotelBooking.DTOs
{
    public class StaffShiftCreateDto
    {
        public int StaffId { get; set; }
        public DateTime ShiftDate { get; set; }
        public string ShiftTime { get; set; }
    }
}