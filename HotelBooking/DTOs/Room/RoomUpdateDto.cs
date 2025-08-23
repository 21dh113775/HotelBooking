using Microsoft.AspNetCore.Http;

namespace HotelBooking.DTOs.Room
{
    public class RoomUpdateDto
    {
        public string RoomNumber { get; set; } = string.Empty; // Đồng bộ với RoomCreateDto
        public double PricePerNight { get; set; } // Đồng bộ
        public bool IsAvailable { get; set; } // Đồng bộ
        public string? Description { get; set; } // Giữ nguyên
        public decimal? PricePerHour { get; set; } // Thêm nếu cần, nullable để tùy chọn
        public IFormFile? Image { get; set; } // Giữ nguyên cho upload
    }
}