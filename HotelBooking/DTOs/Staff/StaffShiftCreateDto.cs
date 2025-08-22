using System;

namespace HotelBooking.DTOs
{
    // === GIẢI THÍCH: DTO dùng để tạo ca làm việc mới cho staff ===
    // Lý do: Tách biệt dữ liệu đầu vào từ entity StaffShift, để dễ validate và map, phù hợp logic thực tế (admin gán ca mà không cần truyền full entity) ===
    public class StaffShiftCreateDto
    {
        // === GIẢI THÍCH: ID của staff (user có role Staff) ===
        // Lý do: Xác định staff nào được gán ca, phải lớn hơn 0 và tồn tại trong DB ===
        public int StaffId { get; set; }

        // === GIẢI THÍCH: Ngày ca làm việc ===
        // Lý do: Phải sau ngày hiện tại để tránh gán ca quá khứ ===
        public DateTime ShiftDate { get; set; }

        // === GIẢI THÍCH: Thời gian ca (ví dụ: Morning, Afternoon, Night) ===
        // Lý do: Giới hạn các giá trị hợp lệ để dễ quản lý lịch trình ===
        public string ShiftTime { get; set; }
    }
}