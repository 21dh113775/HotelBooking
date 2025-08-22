namespace HotelBooking.DTOs
{
    public class AdminUserUpdateDto
    {
        public int UserId { get; set; }
        public int RoleId { get; set; } // ID role mới để thay đổi phân quyền
    }
}