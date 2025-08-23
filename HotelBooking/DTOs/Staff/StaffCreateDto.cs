using System.ComponentModel.DataAnnotations;

namespace HotelBooking.DTOs
{
    public class StaffCreateDto
    {
        [Required]
        public string FullName { get; set; }
        [Required, EmailAddress]
        public string Email { get; set; }
        [Required, Phone]
        public string PhoneNumber { get; set; }
        [Required]
        public string Password { get; set; }
        // Xóa role vì đã mặc định RoleId=3 trong controller
    }
}